namespace Magnum.ProtocolBuffers.Specs
{
    using Internal;
    using NUnit.Framework;
    using TestMessages;

    [TestFixture]
    public class String_mapping
    {
        [Test]
        public void Does_it_match()
        {
            var map = new MessageMap<SearchRequest>();
            map.Field(m=>m.Query).MakeRequired();
            map.Field(m => m.PageNumber);
            map.Field(m=>m.ResultPerPage).SetDefaultValue(10);

            IMapping mm =  map;
            StringVisitor v = new StringVisitor();
            mm.Visit(v);

            string proto = @"message SearchRequest {
  required string query = 1;
  optional int32 page_number = 2;
  optional int32 result_per_page = 3 [default = 10];
}
";
            Assert.AreEqual(proto, v.GetMap());
        }
        
    }
}