namespace UD.Core.Helper.Results
{
    public sealed class LeftJoinResult<T, Y>
    {
        public T left { get; set; }
        public bool hasRight { get; set; }
        public Y right { get; set; }
    }
}