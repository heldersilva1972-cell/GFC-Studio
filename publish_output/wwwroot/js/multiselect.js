// Helper function for Blazor multi-select elements
window.getSelectedValues = (element) => {
    if (!element) {
        return [];
    }
    const selectElement = element;
    const selectedOptions = Array.from(selectElement.selectedOptions);
    return selectedOptions.map(option => option.value);
};
