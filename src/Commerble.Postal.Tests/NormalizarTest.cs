using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace Commerble.Postal.Tests
{
    [TestClass]
    public class NormalizarTest
    {
        [TestMethod]
        public void 除外ワード()
        {
            var inputs = new[]{
                "吉岡（成田国際空港内）",
                "中央アエル（地階・階層不明）",
                "三里塚（御料牧場・成田国際空港内）",
                "高田町（３丁目東・西・南）",
                "北一条西（２０～２８丁目）",
                "あけぼの町（１、２丁目）",
                "はまなす町（１丁目）",
                "八幡（５丁目１番地）",
                "大江（１丁目、２丁目「６５１、６６２、６６８番地」以外、３丁目５、１３－４、２０、６７８、６８７番地）",
                "宇尾町（８９７番地及び中島５０５～５１８番地）",
                "岩倉上蔵町（２６７、２６８番地）",
                "岩倉木野町（２５１番地）",
                "上石切町（２丁目８６３、１４５１番地）",
                "山田町下谷上（大上谷、修法ケ原、中一里山「９番地の４、１２番地を除く」長尾山、再度公園）",
                "山田町下谷上（菊水山、高座川向、中一里山「９番地の４、１２番地」念仏堂、ひよどり越）",
                "唐桑町西舞根（２００番以上）",
                "南山（４３０番地以上「１７７０－１～２、１８６２－４２、１９２３－５を除く」、大谷地、折渡、鍵金野、金山、滝ノ沢、豊牧、沼の台、肘折、平林）",
                "犬落瀬（内金矢、内山、岡沼、金沢、金矢、上淋代、木越、権現沢、四木、七百、下久保「１７４を除く」、下淋代、高森、通目木、坪毛沢「２５、６３７、６４１、６４３、６４７を除く」、中屋敷、沼久保、根古橋、堀切沢、南平、柳沢、大曲）",
                "折茂（今熊「２１３～２３４、２４０、２４７、２６２、２６６、２７５、２７７、２８０、２９５、１１９９、１２０６、１５０４を除く」、大原、沖山、上折茂「１－１３、７１－１９２を除く」）",
                "葛巻（第４０地割「５７番地１２５、１７６を除く」～第４５地割）",
                "板屋町（次のビルを除く）",
                "東洋（油駒、南東洋、１３２～１５６、１５８～３５４、３６６、３６７番地）",
                "俣落（１６２９－１、１６５３－３、１６８４－５、１８８６－２、１８９３、１８９６－２、１８９６－３、１９０６－８、１９３２－２、１９３６、１９３６－２、１９５１－２、１９５６－２、１９６７－２、１９８０－２、１９９７－３、２０００－２、２０００－８、２００３、２０１５－４、２０２３－２、２０３８－２、２２５３－２、２２５３－５、２２５３－２１、２２５３－２４、２２５３－３５、２２５３－６３）",
                "第４０地割「５７番地１２５、１７６を除く」～第４５地割",
            };
            var results = new[] {
                "吉岡",
                "中央アエル",
                "三里塚",
                "高田町",
                "北一条西",
                "あけぼの町",
                "はまなす町",
                "八幡",
                "大江",
                "宇尾町",
                "岩倉上蔵町",
                "岩倉木野町",
                "上石切町",
                "山田町下谷上（大上谷、修法ケ原、中一里山長尾山、再度公園）",
                "山田町下谷上（菊水山、高座川向、中一里山念仏堂、ひよどり越）",
                "唐桑町西舞根",
                "南山",
                "犬落瀬（内金矢、内山、岡沼、金沢、金矢、上淋代、木越、権現沢、四木、七百、下久保、下淋代、高森、通目木、坪毛沢、中屋敷、沼久保、根古橋、堀切沢、南平、柳沢、大曲）",
                "折茂（今熊、大原、沖山、上折茂）",
                "葛巻",
                "板屋町",
                "東洋",
                "俣落（１６２９－１、１６５３－３、１６８４－５、１８８６－２、１８９３、１８９６－２、１８９６－３、１９０６－８、１９３２－２、１９３６、１９３６－２、１９５１－２、１９５６－２、１９６７－２、１９８０－２、１９９７－３、２０００－２、２０００－８、２００３、２０１５－４、２０２３－２、２０３８－２、２２５３－２、２２５３－５、２２５３－２１、２２５３－２４、２２５３－３５、２２５３－６３）",
                "",
            };
            var normalizar = new PostalNormalizar();
            for (var i = 0; i < inputs.Length; i++)
            {
                var excepted = normalizar.Except(inputs[i]);
                Assert.AreEqual(results[i], excepted);
            }
        }

        [TestMethod]
        public void Street分割()
        {
            var inputs = new[] {
                "旭ケ丘",
                "１～１９丁目",
                "４００、４００－２番地",
                "上勇知、下勇知、夕来、オネトマナイ",
                "１６２９－１、１６５３－３、１６８４－５、１８８６－２、１８９３、１８９６－２、１８９６－３、１９０６－８、１９３２－２、１９３６、１９３６－２、１９５１－２、１９５６－２、１９６７－２、１９８０－２、１９９７－３、２０００－２、２０００－８、２００３、２０１５－４、２０２３－２、２０３８－２、２２５３－２、２２５３－５、２２５３－２１、２２５３－２４、２２５３－３５、２２５３－６３",
            };
            var results = new[] {
                new[]{"旭ケ丘" },
                new string[]{},
                new[]{ "４００","４００－２番地" },
                new[]{ "上勇知", "下勇知", "夕来", "オネトマナイ" },
                new[]{ "１６２９－１", "１６５３－３", "１６８４－５", "１８８６－２", "１８９３", "１８９６－２", "１８９６－３", "１９０６－８", "１９３２－２", "１９３６", "１９３６－２", "１９５１－２", "１９５６－２", "１９６７－２", "１９８０－２", "１９９７－３", "２０００－２", "２０００－８", "２００３", "２０１５－４", "２０２３－２", "２０３８－２", "２２５３－２", "２２５３－５", "２２５３－２１", "２２５３－２４", "２２５３－３５", "２２５３－６３" },
                new string[]{},
            };

            var normalizar = new PostalNormalizar();
            for (var i = 0; i < inputs.Length; i++)
            {
                var parts = normalizar.Parts(inputs[i]);
                Assert.AreEqual(string.Join(",", results[i]), string.Join(",", parts));
            }
        }

        [TestMethod]
        public void 以下に掲載がない場合()
        {
            var inputs = new[] { "01101,\"060  \",\"0600000\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾁｭｳｵｳｸ\",\"ｲｶﾆｹｲｻｲｶﾞﾅｲﾊﾞｱｲ\",\"北海道\",\"札幌市中央区\",\"以下に掲載がない場合\",0,0,0,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("0600000", postal.Code);
            Assert.AreEqual("", postal.Street);
        }

        [TestMethod]
        public void 地階階層不明()
        {
            var inputs = new[] { "04101,\"980  \",\"9806190\",\"ﾐﾔｷﾞｹﾝ\",\"ｾﾝﾀﾞｲｼｱｵﾊﾞｸ\",\"ﾁｭｳｵｳｱｴﾙ(ﾁｶｲ･ｶｲｿｳﾌﾒｲ)\",\"宮城県\",\"仙台市青葉区\",\"中央アエル（地階・階層不明）\",0,0,0,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("中央アエル", postal.Street);
        }

        [TestMethod]
        public void 番地その１()
        {
            var inputs = new[] { "01106,\"005  \",\"0050865\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾐﾅﾐｸ\",\"ﾄｷﾜ(1-131ﾊﾞﾝﾁ)\",\"北海道\",\"札幌市南区\",\"常盤（１～１３１番地）\",1,0,0,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("常盤", postal.Street);
        }

        [TestMethod]
        public void 番地その２()
        {
            var inputs = new[] { "01106,\"005  \",\"0050840\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾐﾅﾐｸ\",\"ﾌｼﾞﾉ(400､400-2ﾊﾞﾝﾁ)\",\"北海道\",\"札幌市南区\",\"藤野（４００、４００－２番地）\",1,0,0,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("藤野", postal.Street);
        }

        [TestMethod]
        public void 番地その３()
        {
            var inputs = new[] {
                "28109,\"65111\",\"6511102\",\"ﾋｮｳｺﾞｹﾝ\",\"ｺｳﾍﾞｼｷﾀｸ\",\"ﾔﾏﾀﾞﾁｮｳｼﾓﾀﾆｶﾞﾐ(ｵｵｶﾐﾀﾞﾆ､ｼｭｳﾎｳｶﾞﾊﾗ､ﾅｶｲﾁﾘﾔﾏ<9ﾊﾞﾝﾁﾉ4､12ﾊﾞﾝﾁｦﾉｿﾞｸ>ﾅｶﾞ\",\"兵庫県\",\"神戸市北区\",\"山田町下谷上（大上谷、修法ケ原、中一里山「９番地の４、１２番地を除く」長\",1,1,0,0,0,0",
                "28109,\"65111\",\"6511102\",\"ﾋｮｳｺﾞｹﾝ\",\"ｺｳﾍﾞｼｷﾀｸ\",\"ｵﾔﾏ､ﾌﾀﾀﾋﾞｺｳｴﾝ)\",\"兵庫県\",\"神戸市北区\",\"尾山、再度公園）\",1,1,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "山田町下谷上", "山田町下谷上大上谷", "山田町下谷上修法ケ原", "山田町下谷上中一里山長尾山", "山田町下谷上再度公園" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 丁目その１()
        {
            var inputs = new[] { "01101,\"060  \",\"0600042\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾁｭｳｵｳｸ\",\"ｵｵﾄﾞｵﾘﾆｼ(1-19ﾁｮｳﾒ)\",\"北海道\",\"札幌市中央区\",\"大通西（１～１９丁目）\",1,0,1,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("大通西", postal.Street);
        }

        [TestMethod]
        public void 丁目その２()
        {
            var inputs = new[] { "01106,\"005  \",\"0050030\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾐﾅﾐｸ\",\"ﾐﾅﾐ30ｼﾞｮｳﾆｼ(8ﾁｮｳﾒ)\",\"北海道\",\"札幌市南区\",\"南三十条西（８丁目）\",0,0,1,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("南三十条西", postal.Street);
        }

        [TestMethod]
        public void 丁目その３()
        {
            var inputs = new[] { "03205,\"025  \",\"0250056\",\"ｲﾜﾃｹﾝ\",\"ﾊﾅﾏｷｼ\",\"ｶﾐｷﾀﾏﾝﾁｮｳﾒ\",\"岩手県\",\"花巻市\",\"上北万丁目\",0,0,0,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("上北万丁目", postal.Street);
        }

        [TestMethod]
        public void 丁目その４()
        {
            var inputs = new[] { "05214,\"01801\",\"0180126\",\"ｱｷﾀｹﾝ\",\"ﾆｶﾎｼ\",\"ｷｻｶﾀﾏﾁ1ﾁｮｳﾒｼｵｺｼ\",\"秋田県\",\"にかほ市\",\"象潟町１丁目塩越\",0,0,0,0,0,0" };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("象潟町１丁目塩越", postal.Street);
        }

        [TestMethod]
        public void 丁目その５()
        {
            var inputs = new[] {
                "01407,\"04824\",\"0482402\",\"ﾎｯｶｲﾄﾞｳ\",\"ﾖｲﾁｸﾞﾝﾆｷﾁｮｳ\",\"ｵｵｴ(1ﾁｮｳﾒ､2ﾁｮｳﾒ<651､662､668ﾊﾞﾝﾁ>ｲｶﾞｲ､3ﾁｮｳﾒ5､1\",\"北海道\",\"余市郡仁木町\",\"大江（１丁目、２丁目「６５１、６６２、６６８番地」以外、３丁目５、１\",1,0,1,0,0,0",
                "01407,\"04824\",\"0482402\",\"ﾎｯｶｲﾄﾞｳ\",\"ﾖｲﾁｸﾞﾝﾆｷﾁｮｳ\",\"3-4､20､678､687ﾊﾞﾝﾁ)\",\"北海道\",\"余市郡仁木町\",\"３－４、２０、６７８、６８７番地）\",1,0,1,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "大江" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その１()
        {
            var inputs = new[] {
                "03202,\"02824\",\"0282402\",\"ｲﾜﾃｹﾝ\",\"ﾐﾔｺｼ\",\"ｶﾜｲ(ﾀﾞｲ9ﾁﾜﾘ-ﾀﾞｲ11ﾁﾜﾘ)\",\"岩手県\",\"宮古市\",\"川井（第９地割～第１１地割）\",1,1,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "川井" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その２()
        {
            var inputs = new[] {
                "03202,\"02825\",\"0282504\",\"ｲﾜﾃｹﾝ\",\"ﾐﾔｺｼ\",\"ﾊｺｲｼ(ﾀﾞｲ2ﾁﾜﾘ<70-136>-ﾀﾞｲ4ﾁﾜﾘ<3-11>)\",\"岩手県\",\"宮古市\",\"箱石（第２地割「７０～１３６」～第４地割「３～１１」）\",1,1,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "箱石" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その３()
        {
            var inputs = new[] {
                "03302,\"02851\",\"0285102\",\"ｲﾜﾃｹﾝ\",\"ｲﾜﾃｸﾞﾝｸｽﾞﾏｷﾏﾁ\",\"ｸｽﾞﾏｷ(ﾀﾞｲ40ﾁﾜﾘ<57ﾊﾞﾝﾁ125､176ｦﾉｿﾞｸ>-ﾀﾞｲ45\",\"岩手県\",\"岩手郡葛巻町\",\"葛巻（第４０地割「５７番地１２５、１７６を除く」～第４５\",1,1,0,0,0,0",
                "03302,\"02851\",\"0285102\",\"ｲﾜﾃｹﾝ\",\"ｲﾜﾃｸﾞﾝｸｽﾞﾏｷﾏﾁ\",\"ﾁﾜﾘ)\",\"岩手県\",\"岩手郡葛巻町\",\"地割）\",1,1,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "葛巻" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その４()
        {
            var inputs = new[] {
                "03366,\"02955\",\"0295503\",\"ｲﾜﾃｹﾝ\",\"ﾜｶﾞｸﾞﾝﾆｼﾜｶﾞﾏﾁ\",\"ｱﾅｱｹ22ﾁﾜﾘ､ｱﾅｱｹ23ﾁﾜﾘ\",\"岩手県\",\"和賀郡西和賀町\",\"穴明２２地割、穴明２３地割\",0,0,0,1,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "穴明" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その５()
        {
            var inputs = new[] {
                "03366,\"02955\",\"0295523\",\"ｲﾜﾃｹﾝ\",\"ﾜｶﾞｸﾞﾝﾆｼﾜｶﾞﾏﾁ\",\"ｴｯﾁｭｳﾊﾀ64ﾁﾜﾘ-ｴｯﾁｭｳﾊﾀ66ﾁﾜﾘ\",\"岩手県\",\"和賀郡西和賀町\",\"越中畑６４地割～越中畑６６地割\",0,0,0,1,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "越中畑" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その６()
        {
            var inputs = new[] {
                "03366,\"02955\",\"0295507\",\"ｲﾜﾃｹﾝ\",\"ﾜｶﾞｸﾞﾝﾆｼﾜｶﾞﾏﾁ\",\"ｵｵｸﾂ36ﾁﾜﾘ\",\"岩手県\",\"和賀郡西和賀町\",\"大沓３６地割\",0,0,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "大沓３６地割" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その７()
        {
            var inputs = new[] {
                "03366,\"02955\",\"0240341\",\"ｲﾜﾃｹﾝ\",\"ﾜｶﾞｸﾞﾝﾆｼﾜｶﾞﾏﾁ\",\"ｽｷﾞﾅﾊﾀ44ﾁﾜﾘ(ﾕﾀﾞﾀﾞﾑｶﾝﾘｼﾞﾑｼｮ､ｳｼﾛｸﾞﾁﾔﾏ､ｱﾃﾗｸ)\",\"岩手県\",\"和賀郡西和賀町\",\"杉名畑４４地割（湯田ダム管理事務所、後口山、当楽）\",1,0,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "杉名畑４４地割", "杉名畑４４地割湯田ダム管理事務所", "杉名畑４４地割後口山", "杉名畑４４地割当楽" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 地割その８()
        {
            var inputs = new[] {
                "03507,\"02879\",\"0287915\",\"ｲﾜﾃｹﾝ\",\"ｸﾉﾍｸﾞﾝﾋﾛﾉﾁｮｳ\",\"ﾀﾈｲﾁﾀﾞｲ15ﾁﾜﾘ-ﾀﾞｲ21ﾁﾜﾘ(ｶﾇｶ､ｼｮｳｼﾞｱｲ､ﾐﾄﾞﾘﾁｮｳ､ｵｵｸﾎﾞ､ﾀｶﾄﾘ)\",\"岩手県\",\"九戸郡洋野町\",\"種市第１５地割～第２１地割（鹿糠、小路合、緑町、大久保、高取）\",0,1,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            var streets = normalized.Select(p => p.Street);
            var results = new[] { "種市" };
            Assert.AreEqual(string.Join(",", streets), string.Join(",", results));
        }

        [TestMethod]
        public void 事業所()
        {
            var inputs = new[] {
                "01101,\"(ｶﾌﾞ) ﾆﾎﾝｹｲｻﾞｲｼﾝﾌﾞﾝｼﾔ ｻﾂﾎﾟﾛｼｼﾔ\",\"株式会社日本経済新聞社札幌支社\",\"北海道\",\"札幌市中央区\",\"北一条西\",\"６丁目１－２アーバンネット札幌ビル２Ｆ\",\"0608621\",\"060  \",\"札幌中央\",0,0,0"
            };
            var postal = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Jigyosyo)).First();

            Assert.AreEqual("株式会社日本経済新聞社札幌支社", postal.Name);
        }

        [TestMethod]
        public void 山梨県北都留郡小菅村()
        {
            var inputs = new[] {
                "19442,\"40902\",\"4090200\",\"ﾔﾏﾅｼｹﾝ\",\"ｷﾀﾂﾙｸﾞﾝｺｽｹﾞﾑﾗ\",\"ｲｶﾆｹｲｻｲｶﾞﾅｲﾊﾞｱｲ\",\"山梨県\",\"北都留郡小菅村\",\"以下に掲載がない場合\",0,0,0,0,0,0",
                "19442,\"40902\",\"4090142\",\"ﾔﾏﾅｼｹﾝ\",\"ｷﾀﾂﾙｸﾞﾝｺｽｹﾞﾑﾗ\",\"ｺｽｹﾞﾑﾗﾉﾂｷﾞﾆ1-663ﾊﾞﾝﾁｶﾞｸﾙﾊﾞｱｲ\",\"山梨県\",\"北都留郡小菅村\",\"小菅村の次に１～６６３番地がくる場合\",1,0,0,0,0,0",
                "19442,\"40902\",\"4090211\",\"ﾔﾏﾅｼｹﾝ\",\"ｷﾀﾂﾙｸﾞﾝｺｽｹﾞﾑﾗ\",\"ｺｽｹﾞﾑﾗﾉﾂｷﾞﾆ664ﾊﾞﾝﾁｲｺｳｶﾞｸﾙﾊﾞｱｲ\",\"山梨県\",\"北都留郡小菅村\",\"小菅村の次に６６４番地以降がくる場合\",1,0,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals).ToArray();

            var streets = normalized.Where(p => p.Street.Contains("小菅村")).Select(p => p.Street);
            Assert.AreEqual(string.Join(",", streets), "");
        }

        [TestMethod]
        public void 香川県仲多度郡琴平町()
        {
            var inputs = new[] {
                "37403,\"766  \",\"7660000\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｲｶﾆｹｲｻｲｶﾞﾅｲﾊﾞｱｲ\",\"香川県\",\"仲多度郡琴平町\",\"以下に掲載がない場合\",0,0,0,1,0,0",
                "37403,\"766  \",\"7660004\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｴﾅｲ\",\"香川県\",\"仲多度郡琴平町\",\"榎井\",0,0,0,0,0,0",
                "37403,\"766  \",\"7660006\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｶﾐｸｼﾅｼ\",\"香川県\",\"仲多度郡琴平町\",\"上櫛梨\",0,0,0,0,0,0",
                "37403,\"766  \",\"7660002\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｺﾄﾋﾗﾁｮｳ(1-426ﾊﾞﾝﾁ､ｶﾜﾋｶﾞｼ)\",\"香川県\",\"仲多度郡琴平町\",\"琴平町（１～４２６番地、川東）\",1,0,0,0,0,0",
                "37403,\"766  \",\"7660001\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｺﾄﾋﾗﾁｮｳ(427ﾊﾞﾝﾁｲｼﾞｮｳ､ｶﾜﾆｼ)\",\"香川県\",\"仲多度郡琴平町\",\"琴平町（４２７番地以上、川西）\",1,0,0,0,0,0",
                "37403,\"766  \",\"7660003\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｺﾞｼﾞｮｳ\",\"香川県\",\"仲多度郡琴平町\",\"五條\",0,0,0,0,0,0",
                "37403,\"766  \",\"7660007\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｼﾓｸｼﾅｼ\",\"香川県\",\"仲多度郡琴平町\",\"下櫛梨\",0,0,0,0,0,0",
                "37403,\"766  \",\"7660005\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ﾉｳﾀﾞ\",\"香川県\",\"仲多度郡琴平町\",\"苗田\",0,0,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals).ToArray();

            var streets = normalized.Where(p => p.Street.Contains("琴平町")).Select(p => p.Street);
            Assert.AreEqual(string.Join(",", streets), "");
        }

        [TestMethod]
        public void 香川県仲多度郡琴平町2022()
        {
            var inputs = new[] {
                "37403,\"766  \",\"7660002\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｺﾄﾋﾗﾁｮｳﾉﾂｷﾞﾆ1-426ﾊﾞﾝﾁｶﾞｸﾙﾊﾞｱｲ(ｶﾜﾋｶﾞｼ)\",\"香川県\",\"仲多度郡琴平町\",\"琴平町の次に１～４２６番地がくる場合（川東）\",1,0,0,0,0,0",
                "37403,\"766  \",\"7660001\",\"ｶｶﾞﾜｹﾝ\",\"ﾅｶﾀﾄﾞｸﾞﾝｺﾄﾋﾗﾁｮｳ\",\"ｺﾄﾋﾗﾁｮｳﾉﾂｷﾞﾆ427ﾊﾞﾝﾁｲｺｳｶﾞｸﾙﾊﾞｱｲ(ｶﾜﾆｼ)\",\"香川県\",\"仲多度郡琴平町\",\"琴平町の次に４２７番地以降がくる場合（川西）\",1,0,0,0,0,0"
            };
            var postals = inputs.Select(_ => PostalLoader.Parse(_, ParseMode.Ken));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals).ToArray();

            var streets = normalized.Where(p => p.Street.Contains("琴平町")).Select(p => p.Street);
            Assert.AreEqual(string.Join(",", streets), "");
        }

        [TestMethod]
        public void 全件抽出()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var postalFetcher = new PostalFetcher(AppDomain.CurrentDomain.BaseDirectory);
            postalFetcher.LoadKen();
            postalFetcher.LoadJigyosyo();

            var raw = postalFetcher.RawPostalList.Select(p => p.Code).Distinct().OrderBy(p => p);
            var normalized = postalFetcher.NormalizedPostalList.Select(p => p.Code).Distinct().OrderBy(p => p);

            Console.WriteLine("raw: {0}件", raw.Count());
            Console.WriteLine("normalized: {0}件", normalized.Count());

            var diff = raw.Except(normalized);
            Console.WriteLine("raw - normalized: {0}件", diff.Count());
            foreach (var code in diff)
            {
                Console.WriteLine(code);
            }

            Assert.IsTrue(raw.SequenceEqual(normalized));
        }
    }
}
