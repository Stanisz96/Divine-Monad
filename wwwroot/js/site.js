function ReloadComponent(componentId, action, controller, data) {
    $.ajax({
        url: `/${controller}/${action}`,
        data: data,
        success: function (data) { $("#" + componentId).html(data); }
    });
};
