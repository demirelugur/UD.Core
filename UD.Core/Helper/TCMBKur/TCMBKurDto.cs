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
        private int _unit;
        private decimal _forexBuying;
        private decimal _forexSelling;
        private decimal _banknoteBuying;
        private decimal _banknoteSelling;
        public int Unit { get { return _unit; } set { _unit = value; } }
        public decimal ForexBuying { get { return _forexBuying; } set { _forexBuying = value; } }
        public decimal ForexSelling { get { return _forexSelling; } set { _forexSelling = value; } }
        public decimal BanknoteBuying { get { return _banknoteBuying; } set { _banknoteBuying = value; } }
        public decimal BanknoteSelling { get { return _banknoteSelling; } set { _banknoteSelling = value; } }
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
