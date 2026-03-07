namespace UD.Core.Helper.TCMBKur
{
    using System;
    public sealed class TCMBKurDto : IEquatable<TCMBKurDto>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as TCMBKurDto);
        public override int GetHashCode() => HashCode.Combine(this.Unit, this.ForexBuying, this.ForexSelling, this.BanknoteBuying, this.BanknoteSelling);
        public bool Equals(TCMBKurDto other) => (other != null && this.Unit == other.Unit && this.ForexBuying == other.ForexBuying && this.ForexSelling == other.ForexSelling && this.BanknoteBuying == other.BanknoteBuying && this.BanknoteSelling == other.BanknoteSelling);
        #endregion
        public int Unit { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
        public decimal BanknoteBuying { get; set; }
        public decimal BanknoteSelling { get; set; }
        public TCMBKurDto() : this(default, default, default, default, default) { }
        public TCMBKurDto(int Unit, decimal ForexBuying, decimal ForexSelling, decimal BanknoteBuying, decimal BanknoteSelling)
        {
            this.Unit = Unit;
            this.ForexBuying = ForexBuying;
            this.ForexSelling = ForexSelling;
            this.BanknoteBuying = BanknoteBuying;
            this.BanknoteSelling = BanknoteSelling;
        }
    }
}
