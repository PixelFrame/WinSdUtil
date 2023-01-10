using WinSdUtil.Lib.Model.Binary;

namespace WinSdUtil.Lib.Model
{
    public class BinarySecurityDescriptor
    {
        public byte[] Value { get => SD.GetBytes(); set { SD = new(value); } }
        internal SecurityDescriptor SD { get; set; }

        public BinarySecurityDescriptor(byte[] value) { Value = value; }

        internal BinarySecurityDescriptor(SecurityDescriptor sd) { SD = sd; }

        public SDDL ToSDDL() { return this.ToACL().ToSDDL(); }

        public AccessControlList ToACL()
        {
            return new AccessControlList(this);
        }
    }
}
