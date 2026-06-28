const beautifyPhoneNumber = (value) => {
    if (isNullOrEmpty(value)) {
        return "";
    }
    let s = String(value).trim();
    if (s.length !== 10) {
        return "";
    }
    if (!/^\d+$/.test(s)) {
        return "";
    }
    return `(${s.substring(0, 3)}) ${s.substring(3, 6)}-${s.substring(6, 10)}`;
};
const isObject = (value) => {
    if (value === null) {
        return false;
    }
    return typeof value === "object" && !Array.isArray(value);
};
const isNullOrEmpty = (value) => {
    if (typeof value === "undefined") {
        return true;
    }
    if (value === null) {
        return true;
    }
    if (typeof value === "string") {
        let trimmedValue = value.trim();
        if (trimmedValue.length === 0) {
            return true;
        }
        if (trimmedValue === "00000000-0000-0000-0000-000000000000") {
            return true;
        }
        if (
            ["undefined", "null"].includes(
                trimmedValue.toLowerCase()
            )
        ) {
            return true;
        }
        return false;
    }
    if (isObject(value) && Object.keys(value).length === 0) {
        return true;
    }
    if (Array.isArray(value) && value.length === 0) {
        return true;
    }
    return false;
};
const isNotNullOrEmpty = (value) => {
    return !isNullOrEmpty(value);
};
const isValidPhoneNumber = (value) => {
    if (isNullOrEmpty(value)) {
        return false;
    }
    let cleanedValue = value.replace(/[()\-\s]/g, "");
    let phoneRegex = /^\d{10}$/;
    return phoneRegex.test(cleanedValue);
};
const isValidEmail = (value) => {
    if (isNullOrEmpty(value)) {
        return false;
    }
    let emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(value);
};
const isValidGuid = (value) => {
    value = String(value || "").trim();
    if (value.length !== 36) {
        return false;
    }
    let guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(value);
};
const generateGuid = () => {
    if (
        typeof crypto === "object" &&
        typeof crypto.randomUUID === "function"
    ) {
        return crypto.randomUUID();
    }
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
        let r = (Math.random() * 16) | 0;
        let v = c === "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
};
const decodeTokenPayload = (token) => {
    if (isNullOrEmpty(token)) {
        return null;
    }

    let payloadPart = token.split(".")[1];
    if (isNullOrEmpty(payloadPart)) {
        return null;
    }

    try {
        let base64 = payloadPart.replace(/-/g, "+").replace(/_/g, "/");
        let padded = base64.padEnd(
            base64.length + ((4 - (base64.length % 4)) % 4),
            "=",
        );
        let json = atob(padded);
        return JSON.parse(json);
    } catch {
        return null;
    }
};
export const objectHelper = {
    beautifyPhoneNumber,
    isObject,
    isNullOrEmpty,
    isNotNullOrEmpty,
    isValidPhoneNumber,
    isValidEmail,
    isValidGuid,
    generateGuid,
    decodeTokenPayload
};