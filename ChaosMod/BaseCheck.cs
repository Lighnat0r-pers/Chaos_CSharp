namespace ChaosMod
{
    class BaseCheck : ICheck
    {
        static public string Abort => "abort";
        static public string Suspend => "suspend";

        private MemoryAddress address;
        private dynamic failCase;

        public string onFail { get; private set; }

        public BaseCheck(MemoryAddress address, string failCase, string onFail)
        {
            this.address = address;
            this.failCase = address.ConvertToRightDataType(failCase);
            this.onFail = onFail;
        }

        public bool Succeeds()
        {
            return address.Read() != failCase;
        }
    }
}
