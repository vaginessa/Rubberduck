using Rubberduck.VBEditor.SafeComWrappers.Abstract;
using VB = Microsoft.Vbe.Interop;

namespace Rubberduck.VBEditor.SafeComWrappers.VBA
{
    public class Reference : SafeComWrapper<VB.Reference>, IReference
    {
        public Reference(VB.Reference target) 
            : base(target)
        {
        }

        public string Name
        {
            get { return IsWrappingNullReference ? string.Empty : Target.Name; }
        }

        public string Guid
        {
            get { return IsWrappingNullReference ? string.Empty : Target.Guid; }
        }

        public int Major
        {
            get { return IsWrappingNullReference ? 0 : Target.Major; }
        }

        public int Minor
        {
            get { return IsWrappingNullReference ? 0 : Target.Minor; }
        }

        public string Version
        {
            get { return string.Format("{0}.{1}", Major, Minor); }
        }

        public string Description
        {
            get { return IsWrappingNullReference ? string.Empty : Target.Description; }
        }

        public string FullPath
        {
            get { return IsWrappingNullReference ? string.Empty : Target.FullPath; }
        }

        public bool IsBuiltIn
        {
            get { return !IsWrappingNullReference && Target.BuiltIn; }
        }

        public bool IsBroken
        {
            get { return IsWrappingNullReference || Target.IsBroken; }
        }

        public ReferenceKind Type
        {
            get { return IsWrappingNullReference ? 0 : (ReferenceKind)Target.Type; }
        }

        public IReferences Collection
        {
            get { return new References(IsWrappingNullReference ? null : Target.Collection); }
        }

        public IVBE VBE
        {
            get { return new VBE(IsWrappingNullReference ? null : Target.VBE); }
        }

        public override bool Equals(ISafeComWrapper<VB.Reference> other)
        {
            return IsEqualIfNull(other) ||
                   (other != null 
                    && (int)other.Target.Type == (int)Type
                    && other.Target.Name == Name
                    && other.Target.Guid == Guid
                    && other.Target.FullPath == FullPath
                    && other.Target.Major == Major
                    && other.Target.Minor == Minor);
        }

        public bool Equals(IReference other)
        {
            return Equals(other as SafeComWrapper<VB.Reference>);
        }

        public override int GetHashCode()
        {
            return IsWrappingNullReference ? 0 : HashCode.Compute(Type, Name, Guid, FullPath, Major, Minor);
        }
    }
}