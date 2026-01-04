window.getSelectedValues = function (selectElement) {
    if (!selectElement || !selectElement.options) {
        return [];
    }

    const values = [];
    for (let i = 0; i < selectElement.options.length; i++) {
        const opt = selectElement.options[i];
        if (opt.selected) {
            values.push(opt.value ?? opt.text);
        }
    }
    return values;
};
