function ReloadComponent(componentId, action, controller, data) {
    $.ajax({
        url: `/${controller}/${action}`,
        data: data,
        success: function (data) {
            $("#" + componentId).html(data);
            $("#" + componentId).ready(function () {
                console.log(componentId);
                var imgN = $("#" + componentId).find("img").length;
                var imgLoaded = 0

                if (imgN == 0) document.cookie = componentId + "=true";
                if (getCookie(componentId) == "true") updateImageBorders(componentId);

                $("#" + componentId).find("img").on("load", function () {
                    imgLoaded++
                    if (imgLoaded == imgN) updateImageBorders(componentId);
                })
            });
        }
    });
};

function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
    }
    return decodeURI(dc.substring(begin + prefix.length, end));
};


function updateImageBorders(component) {
    $(".big-background .bottom-left").css({ "margin-top": $(".game-plane").height() - 40 })
    $(".big-background .bottom-right").css({ "margin-top": $(".game-plane").height() - 40 })
    $(".big-background .bottom-left").css({ "visibility": "visible" })
    $(".big-background .bottom-right").css({ "visibility": "visible" })
    document.cookie = component + "=true";
};