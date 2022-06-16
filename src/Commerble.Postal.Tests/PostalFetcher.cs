using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace Commerble.Postal.Tests
{
    public class PostalFetcher
    {
        private const string KenUrl = "https://www.post.japanpost.jp/zipcode/dl/kogaki/zip/ken_all.zip";
        private const string KenFileName = "KEN_ALL.CSV";
        private const string JigyosyoUrl = "https://www.post.japanpost.jp/zipcode/dl/jigyosyo/zip/jigyosyo.zip";
        private const string JigyosyoFileName = "JIGYOSYO.CSV";

        private string _workingDir = null;

        public IEnumerable<PostalCode> RawKenList { get; private set; }
        public IEnumerable<PostalCode> RawJigyosyoList { get; private set; }
        public IEnumerable<PostalCode> RawPostalList =>
            (RawKenList ?? Enumerable.Empty<PostalCode>()).Concat(RawJigyosyoList ?? Enumerable.Empty<PostalCode>());
        public IEnumerable<PostalCode> NormalizedKenList { get; private set; }
        public IEnumerable<PostalCode> NormalizedJigyosyoList { get; private set; }
        public IEnumerable<PostalCode> NormalizedPostalList =>
            (NormalizedKenList ?? Enumerable.Empty<PostalCode>()).Concat(NormalizedJigyosyoList ?? Enumerable.Empty<PostalCode>());

        public PostalFetcher(string workingDir)
        {
            _workingDir = Path.Combine(workingDir, DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        public void LoadKen()
        {
            var normalizar = new PostalNormalizar();
            RawKenList = Download(KenUrl, KenFileName, ParseMode.Ken);
            NormalizedKenList = normalizar.Normalize(RawKenList).Distinct();
        }

        public void LoadJigyosyo()
        {
            var normalizar = new PostalNormalizar();
            RawJigyosyoList = Download(JigyosyoUrl, JigyosyoFileName, ParseMode.Jigyosyo);
            NormalizedJigyosyoList = normalizar.Normalize(RawJigyosyoList).Distinct();
        }

        public IEnumerable<PostalCode> Download(string url, string csvFileName, ParseMode mode)
        {
            try
            {
                if (!Directory.Exists(_workingDir))
                    Directory.CreateDirectory(_workingDir);

                var webClient = new WebClient();

                var zipFilePath = Path.Combine(_workingDir, mode.ToString() + ".zip");
                if (!File.Exists(zipFilePath))
                    webClient.DownloadFile(url, zipFilePath);

                var csvFilePath = Path.Combine(_workingDir, csvFileName);
                if (!File.Exists(csvFilePath))
                {
                    using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.FullName.EndsWith(csvFileName))
                                entry.ExtractToFile(csvFilePath);
                        }
                    }
                }

                return PostalLoader.Load(csvFilePath, Encoding.GetEncoding("SHIFT_JIS"), mode);
            }
            catch (Exception ex)
            {
                Trace.Write(new { Message = "Download error.", Exception = ex, url, mode });
                throw;
            }
            finally
            {
                Trace.Write(new { Message = $"Finish: Download", url, mode });
            }
        }

        public void ClearWorkingDir()
        {
            if (Directory.Exists(_workingDir))
                Directory.Delete(_workingDir, true);
        }
    }
}
