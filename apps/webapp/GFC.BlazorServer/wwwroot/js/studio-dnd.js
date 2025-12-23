window.initializeDnd = (dotNetHelper) => {
    const el = document.querySelector('.center-canvas');
    new Sortable(el, {
        animation: 150,
        onEnd: function (evt) {
            const itemIds = [];
            Array.from(el.children).forEach(node => {
                if (node.dataset && node.dataset.id) {
                    itemIds.push(node.dataset.id);
                }
            });
            dotNetHelper.invokeMethodAsync('UpdateSectionOrder', itemIds);
        }
    });
};
