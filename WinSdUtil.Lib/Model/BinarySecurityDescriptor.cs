using WinSdUtil.Lib.Model.Binary;

namespace WinSdUtil.Lib.Model
{
    public class BinarySecurityDescriptor
    {
        public byte[] Value { get; set; }

        public BinarySecurityDescriptor(byte[] value) { Value = value; SD = new(value); }

        public SDDL ToSDDL() { return new SDDL(this); }

        public AccessControlList ToACL() { return this.ToSDDL().ToACL(); }

        internal SecurityDescriptor SD;
    }
}
