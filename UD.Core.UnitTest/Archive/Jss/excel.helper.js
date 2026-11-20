import { objectHelper } from "./object.helper";
import * as XLSX from "xlsx";
const exportToExcel = (props) => {
    const { header, data, excelName, pageTitle } = props; // Not: header string[] biçiminde olmalıdır
    if (!Array.isArray(header)) { throw new Error('Columns must be an array'); }
    if (header.length === 0) { throw new Error('Columns cannot be empty'); }
    if (!Array.isArray(data)) { throw new Error('Data must be an array'); }
    if (data.length === 0) { throw new Error('Data cannot be empty'); }
    let mappedRow;
    let fileName = objectHelper.isNullOrEmpty(excelName) ? 'export' : excelName;
    let pageName = objectHelper.isNullOrEmpty(pageTitle) ? 'Rapor' : pageTitle;
    let rows = data.map((row) => {
        mappedRow = {};
        header.forEach((fieldName, index) => {
            mappedRow[header[index]] = row[fieldName] ?? null;
        });
        return mappedRow;
    });
    let worksheet = XLSX.utils.json_to_sheet(rows, { header });
    let workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, pageName);
    XLSX.writeFile(workbook, fileName.concat('.xlsx'));
    return;
};
export const excelHelper = {
    exportToExcel
};