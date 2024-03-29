﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Commerble.Postal
{
    public class PostalNormalizar
    {
        private IEnumerable<PostalCode> Merge(IEnumerable<PostalCode> postals)
        {
            //郵便番号が一致してる行(カンマで続きデータになってる)を連結
            var list = postals.ToArray();
            var merged = new List<PostalCode>();
            int calcNest(string street)
                => street.ToCharArray().Count(c => c == '（') - street.ToCharArray().Count(c => c == '）');

            for (var i = 0; i < list.Length; i++)
            {
                var current = list[i];
                //開始カッコと終了カッコに差があれば、差がなくなるまで次行をマージし続ける
                var nest = calcNest(current.Street);
                if (nest > 0)
                {
                    for (; i + 1 < list.Length && current.Code == list[i + 1].Code && nest > 0;)
                    {
                        var street = list[i + 1].Street;
                        current.Street += street;
                        nest += calcNest(street);
                        i++;
                    }
                }
                merged.Add(current);
            }

            return merged;
        }

        public string Except(string street)
        {
            var normal = street;
            var regexs = new[]
{
                // 特殊フォーマットな自治体
                Tuple.Create("（キョウワマチ）", "（キョウワマチ）",""),
                Tuple.Create("（成田国際空港内）", "（成田国際空港内）",""),
                Tuple.Create("小菅村", "小菅村.*",""),
                Tuple.Create("琴平町", "琴平町.*",""),
                // 頻出なパターン
                Tuple.Create("の次に", ".*の次に.*",""),
                Tuple.Create("「その他」", "「その他」",""),
                Tuple.Create("（その他）", "（その他）",""),
                Tuple.Create("（番地のみ）", "（番地のみ）",""),
                Tuple.Create("（番地）", "（番地）",""),
                Tuple.Create("（無番地）", "（無番地）",""),
                Tuple.Create("（丁目）", "（丁目）",""),
                Tuple.Create("（地階・階層不明）", "（地階・階層不明）",""),
                Tuple.Create("以下に掲載がない場合", "以下に掲載がない場合",""),
                Tuple.Create("・", "（.*・.*）",""),
                Tuple.Create("を除く", "「[^」]*を除く」", ""),
                Tuple.Create("を除く", "（[^）]*を除く）", ""),
                Tuple.Create("「", "「[０-９～]*」", ""),
                Tuple.Create("階）", "（([０-９]*階)）", "$1"),
                Tuple.Create("の次に番地がくる場合", ".*の次に番地がくる場合", ""),
                Tuple.Create("一円", ".*一円", ""),
                // 番地・丁目・番は救わない(カギカッコを先に削除ね)
                Tuple.Create("「", "「.*[０-９]*(丁目|番地).*」", ""),
                Tuple.Create("（", "（.*[０-９]*(丁目|番地|番).*）", ""),
                Tuple.Create("甲、乙", "甲、乙（[^）]*）", "甲、乙"),
                Tuple.Create("地割～", "（?第?[０-９]*地割～.*$", ""),
                Tuple.Create("地割、", "（?第?[０-９]*地割、.*$", ""),
            };
            foreach (var r in regexs)
            {
                if (normal.Contains(r.Item1))
                    normal = Regex.Replace(normal, r.Item2, r.Item3);
            }
            return normal;
        }

        public IEnumerable<string> Parts(string street)
        {
            var normal = street;
            return normal.Split('、').Where(p => !p.Contains('～') && !string.IsNullOrEmpty(p)).Distinct();
        }

        public IEnumerable<PostalCode> Normalize(IEnumerable<PostalCode> postals)
        {
            var merged = Merge(postals);
            foreach (var postal in merged)
            {
                var street = Except(postal.Street);
                var normal = postal;
                normal.Street = street;
                if (string.IsNullOrEmpty(street))
                {
                    yield return normal;
                    continue;
                }

                var sp = street.IndexOf('（');
                var ep = street.IndexOf('）');

                var mainBody = sp < 0 ? street : street.Substring(0, sp);
                var subBody = sp < 0 ? "" : street.Substring(sp + 1, ep - sp - 1);

                foreach (var main in Parts(mainBody))
                {
                    normal.Street = main;
                    yield return normal;

                    foreach (var sub in Parts(subBody))
                    {
                        normal.Street = main + sub;
                        yield return normal;
                    }
                }
            }
        }
    }
}
