namespace UD.Core.Helper.Results
{
    public sealed class LeftJoinResult<T, Y>
    {
        public T Left { get; set; }
        public bool HasRight { get; set; }
        public Y Right { get; set; }
    }
}