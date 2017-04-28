using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
                "葛巻（第４０地割～第４５地割）",
                "板屋町",
                "東洋",
                "俣落（１６２９－１、１６５３－３、１６８４－５、１８８６－２、１８９３、１８９６－２、１８９６－３、１９０６－８、１９３２－２、１９３６、１９３６－２、１９５１－２、１９５６－２、１９６７－２、１９８０－２、１９９７－３、２０００－２、２０００－８、２００３、２０１５－４、２０２３－２、２０３８－２、２２５３－２、２２５３－５、２２５３－２１、２２５３－２４、２２５３－３５、２２５３－６３）",
                "第４０地割～第４５地割",
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
                "第４０地割～第４５地割",
                "種市第１５地割～第２１地割",
                "穴明２２地割、穴明２３地割",
                "草井沢４７地割",
                "湯田１９地割～湯田２１地割"
            };
            var results = new[] {
                new[]{"旭ケ丘" },
                new string[]{},
                new[]{ "４００","４００－２番地" },
                new[]{ "上勇知", "下勇知", "夕来", "オネトマナイ" },
                new[]{ "１６２９－１", "１６５３－３", "１６８４－５", "１８８６－２", "１８９３", "１８９６－２", "１８９６－３", "１９０６－８", "１９３２－２", "１９３６", "１９３６－２", "１９５１－２", "１９５６－２", "１９６７－２", "１９８０－２", "１９９７－３", "２０００－２", "２０００－８", "２００３", "２０１５－４", "２０２３－２", "２０３８－２", "２２５３－２", "２２５３－５", "２２５３－２１", "２２５３－２４", "２２５３－３５", "２２５３－６３" },
                new string[]{},
                new[]{"種市"},
                new[]{"穴明"},
                new[]{"草井沢４７地割"},
                new[]{"湯田"},
            };

            var normalizar = new PostalNormalizar();
            for (var i = 0; i < inputs.Length; i++)
            {
                var parts = normalizar.Parts(inputs[i]);
                Assert.IsTrue(results[i].SequenceEqual(parts));
            }
        }

        [TestMethod]
        public void 以下に掲載がない場合()
        {
            var inputs = new[] { "01101,\"060  \",\"0600000\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾁｭｳｵｳｸ\",\"ｲｶﾆｹｲｻｲｶﾞﾅｲﾊﾞｱｲ\",\"北海道\",\"札幌市中央区\",\"以下に掲載がない場合\",0,0,0,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("0600000",postal.Code);
            Assert.AreEqual("",postal.Street);
        }

        [TestMethod]
        public void 地階階層不明()
        {
            var inputs = new[] { "04101,\"980  \",\"9806190\",\"ﾐﾔｷﾞｹﾝ\",\"ｾﾝﾀﾞｲｼｱｵﾊﾞｸ\",\"ﾁｭｳｵｳｱｴﾙ(ﾁｶｲ･ｶｲｿｳﾌﾒｲ)\",\"宮城県\",\"仙台市青葉区\",\"中央アエル（地階・階層不明）\",0,0,0,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("中央アエル", postal.Street);
        }

        [TestMethod]
        public void 番地その１()
        {
            var inputs = new[] { "01106,\"005  \",\"0050865\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾐﾅﾐｸ\",\"ﾄｷﾜ(1-131ﾊﾞﾝﾁ)\",\"北海道\",\"札幌市南区\",\"常盤（１～１３１番地）\",1,0,0,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("常盤", postal.Street);
        }

        [TestMethod]
        public void 番地その２()
        {
            var inputs = new[] { "01106,\"005  \",\"0050840\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾐﾅﾐｸ\",\"ﾌｼﾞﾉ(400､400-2ﾊﾞﾝﾁ)\",\"北海道\",\"札幌市南区\",\"藤野（４００、４００－２番地）\",1,0,0,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
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
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            Assert.IsTrue(
                normalized.Select(p=>p.Street).SequenceEqual(
                    new[] { "山田町下谷上", "山田町下谷上大上谷", "山田町下谷上修法ケ原", "山田町下谷上中一里山長尾山", "山田町下谷上再度公園" }));
        }

        [TestMethod]
        public void 丁目その１()
        {
            var inputs = new[] { "01101,\"060  \",\"0600042\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾁｭｳｵｳｸ\",\"ｵｵﾄﾞｵﾘﾆｼ(1-19ﾁｮｳﾒ)\",\"北海道\",\"札幌市中央区\",\"大通西（１～１９丁目）\",1,0,1,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("大通西", postal.Street);
        }

        [TestMethod]
        public void 丁目その２()
        {
            var inputs = new[] { "01106,\"005  \",\"0050030\",\"ﾎｯｶｲﾄﾞｳ\",\"ｻｯﾎﾟﾛｼﾐﾅﾐｸ\",\"ﾐﾅﾐ30ｼﾞｮｳﾆｼ(8ﾁｮｳﾒ)\",\"北海道\",\"札幌市南区\",\"南三十条西（８丁目）\",0,0,1,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("南三十条西", postal.Street);
        }

        [TestMethod]
        public void 丁目その３()
        {
            var inputs = new[] { "03205,\"025  \",\"0250056\",\"ｲﾜﾃｹﾝ\",\"ﾊﾅﾏｷｼ\",\"ｶﾐｷﾀﾏﾝﾁｮｳﾒ\",\"岩手県\",\"花巻市\",\"上北万丁目\",0,0,0,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);
            var postal = normalized.First();

            Assert.AreEqual("上北万丁目", postal.Street);
        }

        [TestMethod]
        public void 丁目その４()
        {
            var inputs = new[] { "05214,\"01801\",\"0180126\",\"ｱｷﾀｹﾝ\",\"ﾆｶﾎｼ\",\"ｷｻｶﾀﾏﾁ1ﾁｮｳﾒｼｵｺｼ\",\"秋田県\",\"にかほ市\",\"象潟町１丁目塩越\",0,0,0,0,0,0" };
            var postals = inputs.Select(PostalLoader.Parse);
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
            var postals = inputs.Select(PostalLoader.Parse);
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals);

            Assert.IsTrue(normalized.Select(p => p.Street).SequenceEqual(new[] { "大江" }));
        }

    }
}
