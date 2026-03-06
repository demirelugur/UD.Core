namespace UD.Core.Helper.Results
{
    public sealed class LeftJoinResult<T, Y>
    {
        private T _Left;
        private bool _HasRight;
        private Y _Right;
        public T left { get { return _Left; } set { _Left = value; } }
        public bool hasRight { get { return _HasRight; } set { _HasRight = value; } }
        public Y right { get { return _Right; } set { _Right = value; } }
    }
}