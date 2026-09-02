namespace UD.Core.Helper.Responses
{
    using System;
    public sealed class TCMBResponse : IEquatable<TCMBResponse>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as TCMBResponse);
        public override int GetHashCode() => HashCode.Combine(this.Unit, this.ForexBuying, this.ForexSelling, this.BanknoteBuying, this.BanknoteSelling);
        public bool Equals(TCMBResponse other) => (other != null && this.Unit == other.Unit && this.ForexBuying == other.ForexBuying && this.ForexSelling == other.ForexSelling && this.BanknoteBuying == other.BanknoteBuying && this.BanknoteSelling == other.BanknoteSelling);
        #endregion
        public int Unit { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
        public decimal BanknoteBuying { get; set; }
        public decimal BanknoteSelling { get; set; }
        public TCMBResponse() : this(default, default, default, default, default) { }
        public TCMBResponse(int unit, decimal forexBuying, decimal forexSelling, decimal banknoteBuying, decimal banknoteSelling)
        {
            this.Unit = unit;
            this.ForexBuying = forexBuying;
            this.ForexSelling = forexSelling;
            this.BanknoteBuying = banknoteBuying;
            this.BanknoteSelling = banknoteSelling;
        }
    }
}