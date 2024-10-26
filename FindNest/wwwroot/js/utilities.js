


function readNumberToVietnamese(num) {
    if (num === 0) return "không";

    const units = ["", "nghìn", "triệu", "tỷ"];
    const digits = ["", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"];

    function readHundreds(number) {
        let result = "";
        const hundreds = Math.floor(number / 100);
        const tens = Math.floor((number % 100) / 10);
        const ones = number % 10;

        if (hundreds > 0) {
            result += digits[hundreds] + " trăm ";
        }

        if (tens > 1) {
            result += digits[tens] + " mươi ";
            if (ones === 1) {
                result += "mốt ";
            } else if (ones === 5) {
                result += "lăm ";
            } else if (ones > 0) {
                result += digits[ones] + " ";
            }
        } else if (tens === 1) {
            result += "mười ";
            if (ones === 5) {
                result += "lăm ";
            } else if (ones > 0) {
                result += digits[ones] + " ";
            }
        } else if (ones > 0) {
            if (hundreds > 0) result += "linh ";
            result += digits[ones] + " ";
        }

        return result.trim();
    }

    let result = "";
    let unitIndex = 0;

    while (num > 0) {
        const group = num % 1000;
        if (group > 0) {
            const groupText = readHundreds(group);
            result = groupText + " " + units[unitIndex] + " " + result;
        }
        unitIndex++;
        num = Math.floor(num / 1000);
    }

    return result.trim();
}


function bindReadNumberToInput(inputElement, outputSelector) {
    const input = $(inputElement).val();
    const number = parseInt(input, 10);
    if (!isNaN(number)) {
        const text = readNumberToVietnamese(number);
        $(outputSelector).text(normalizeAndCapitalizeFirst(text));
    } else {
        $(outputSelector).text('Không');
    }
}

function normalizeAndCapitalizeFirst(str) {
    if (typeof str !== 'string' || str.length === 0) {
        return '';
    }
    str = str.toLowerCase();
    return str.charAt(0).toUpperCase() + str.slice(1);
}
