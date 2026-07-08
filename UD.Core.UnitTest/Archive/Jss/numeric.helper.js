import { objectHelper } from "./object.helper";
const ByteMinValue = 0;
const ByteMaxValue = 255;
const Int16MinValue = -32768;
const Int16MaxValue = 32767;
const Int32MinValue = -2147483648;
const Int32MaxValue = 2147483647;
const Int64MinValue = -9223372036854775808n;
const Int64MaxValue = 9223372036854775807n;
const formatNumberTR = (value, fractionDigits) => {
    value = value ?? 0;
    if (Number.isNaN(value)) { return value; }
    let options = {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
        ...(fractionDigits ?? {})
    };
    return Number(value).toLocaleString('tr-TR', options);
};
const isTCKimlikNo = (tckn) => {
    if (objectHelper.isNullOrEmpty(tckn)) { return false; }
    let s = String(tckn).trim();
    if (!/^\d{11}$/.test(s)) { return false; }
    if (s[0] === '0') { return false; }
    let digits = s.split('').map(d => parseInt(d));
    let sumOdd = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
    let sumEven = digits[1] + digits[3] + digits[5] + digits[7];
    let calc10 = ((sumOdd * 7) - sumEven);
    let digit10 = ((calc10 % 10) + 10) % 10;
    if (digit10 !== digits[9]) { return false; }
    let sumFirst10 = digits.slice(0, 10).reduce((a, b) => a + b, 0);
    if ((sumFirst10 % 10) !== digits[10]) { return false; }
    return true;
};
const isTcknOrVkn = (value) => {
    if (isTCKimlikNo(value)) { return true; }
    if (isVergiKimlikNo(value)) { return true; }
    return false;
};
const isVergiKimlikNo = (vkn) => {
    if (objectHelper.isNullOrEmpty(vkn)) { return false; }
    let s = String(vkn).trim();
    if (!/^\d+$/.test(s)) { return false; }
    let _LEN = 10;
    if (s.length < _LEN) { s = s.padStart(_LEN, '0'); } // Not: 33583636 (8 Rakam) -> 0033583636, 602883151 (9 Rakam) -> 0602883151
    if (s.length !== _LEN) { return false; }
    let digits = s.split('').map(d => parseInt(d));
    let i, t, val, nums = [];
    for (i = 0; i < 9; i++) {
        t = (digits[i] + (9 - i)) % 10;
        val = (t * Math.pow(2, (9 - i))) % 9;
        if (t !== 0 && val === 0) { val = 9; }
        nums.push(val);
    }
    let sum = nums.reduce((a, b) => a + b, 0);
    let check = (10 - (sum % 10)) % 10;
    return check === digits[9];
};
export const numericHelper = {
    ByteMinValue,
    ByteMaxValue,
    Int16MinValue,
    Int16MaxValue,
    Int32MinValue,
    Int32MaxValue,
    Int64MinValue,
    Int64MaxValue,
    formatNumberTR,
    isTCKimlikNo,
    isTcknOrVkn,
    isVergiKimlikNo
};