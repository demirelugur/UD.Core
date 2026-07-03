const GuidEmpty = '00000000-0000-0000-0000-000000000000';
const GuidMaxValue = 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF';
const beautifyPhoneNumber = (value) => {
    if (isNullOrEmpty(value)) { return ''; }
    let s = String(value).trim();
    if (s.length !== 10) { return ''; }
    if (!/^\d+$/.test(s)) { return ''; }
    return `(${s.substring(0, 3)}) ${s.substring(3, 6)}-${s.substring(6, 10)}`;
};
const decodeTokenPayload = (token) => {
    if (isNullOrEmpty(token)) { return null; }
    let split = String(token).trim().split('.');
    let payloadPart = (split.length > 1 ? split[1] : '');
    if (isNullOrEmpty(payloadPart)) { return null; }
    try {
        let base64 = payloadPart.replace(/-/g, '+').replace(/_/g, '/');
        let padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=');
        let json = atob(padded);
        return JSON.parse(json);
    } catch { return null; }
};
const distinct = (array) => {
    return [...new Set(array)];
};
const generateGuid = () => {
    if (typeof crypto === 'object' && typeof crypto.randomUUID === 'function') { return crypto.randomUUID(); }
    let r, v;
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
        r = (Math.random() * 16) | 0;
        v = c === "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
};
const groupBy = (array, ...keySelectors) => { /* Örnek kullanım: Tekli Gruplama: groupBy(users, x => x.city), Çoklu Gruplama: groupBy(users, x => x.city, x => x.age) */
    let key;
    return array.reduce((acc, item) => {
        key = keySelectors.map(selector => selector(item)).join('|');
        (acc[key] ??= []).push(item);
        return acc;
    }, {});
};
const isNotNullOrEmpty = (value) => {
    return !isNullOrEmpty(value);
};
const isNullOrEmpty = (value) => {
    if (typeof value === 'undefined') { return true; }
    if (value === null) { return true; }
    if (typeof value === 'object') {
        if (value instanceof Date) {
            if (isNaN(value.valueOf())) { return true; }
            let minDate = new Date(1753, 0, 1);
            let maxDate = new Date(9999, 11, 31, 23, 59, 59, 999);
            return value <= minDate || value >= maxDate;
        }
        if (Array.isArray(value)) { return value.length === 0; }
        if (Object.getPrototypeOf(value) === Object.prototype) { return Object.keys(value).length === 0; }
    }
    if (typeof value === 'string') {
        let trimmedValue = value.trim();
        if (trimmedValue.length === 0) { return true; }
        if (trimmedValue === GuidEmpty) { return true; }
        if (['undefined', 'null'].includes(trimmedValue.toLowerCase())) { return true; }
    }
    return false;
};
const isObject = (value) => {
    if (value === null) { return false; }
    return (typeof value === 'object' && !Array.isArray(value));
};
const isTokenExpired = (token) => {
    let payload = decodeTokenPayload(token);
    if (isObject(payload) && typeof payload.exp === 'number') { return Date.now() >= (payload.exp * 1000) }
    return true;
};
const isValidEmail = (value) => {
    if (isNullOrEmpty(value)) { return false; }
    let emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(value);
};
const isValidGuid = (value) => {
    if (isNullOrEmpty(value)) { return false; }
    let guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(String(value).trim());
};
const isValidPhoneNumber = (value) => {
    if (isNullOrEmpty(value)) { return false; }
    let cleanedValue = value.replace(/[()\s-]/g, '');
    let phoneRegex = /^\d{10}$/;
    return phoneRegex.test(cleanedValue);
};
const toTitleCase = (value, isWhiteSpace = true, punctuations = [], locale = 'tr-TR') => {
    if (isNullOrEmpty(value)) { return ''; }
    let ch, result = '', newWord = true, separators = new Set(punctuations || []);
    if (isWhiteSpace) { separators.add(' '); }
    value = String(value).trim();
    for (ch of value) {
        if (separators.has(ch)) {
            result += ch;
            newWord = true;
        }
        else if (newWord) {
            result += ch.toLocaleUpperCase(locale);
            newWord = false;
        }
        else { result += ch.toLocaleLowerCase(locale); }
    }
    return result;
};
export const objectHelper = {
    GuidEmpty,
    GuidMaxValue,
    beautifyPhoneNumber,
    decodeTokenPayload,
    distinct,
    generateGuid,
    groupBy,
    isNotNullOrEmpty,
    isNullOrEmpty,
    isObject,
    isTokenExpired,
    isValidEmail,
    isValidGuid,
    isValidPhoneNumber,
    toTitleCase
};