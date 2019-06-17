namespace Commerble.Postal
{
    public struct PostalCode
    {
        public string Jis { get; set; }
        public string Code { get; set; }
        public string Prefecture { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Code} {Prefecture} {City} {Street}";
        }
    }
}
