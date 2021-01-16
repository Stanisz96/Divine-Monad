function ReloadComponent(componentId, action, controller, data) {
    $.ajax({
        url: `/${controller}/${action}`,
        data: data,
        success: function (data) {
            $("#" + componentId).html(data);
            $("#" + componentId).ready(function () {
                var imgN = $("#" + componentId).find("img").length;
                var imgLoaded = 0
                if (imgN == 0) document.cookie = componentId + "=true";
                updateImageBorders(componentId);

                $("#" + componentId).find("img").on("load", function () {
                    imgLoaded++
                    if (imgLoaded == imgN) updateImageBorders(componentId);
                })
            });
        }
    });
};

function getCookie(name) {
    let cookies = document.cookie;
    if (cookies.indexOf(name) >= 0) {
        let sub = cookies.substring(document.cookie.indexOf(name));
        if (sub.indexOf(";") >= 0) {
            sub = sub.substring(0, sub.indexOf(";"))
        }
        return sub.split("=")[1];
    }
    return null;
/*    var prefix = name + "=";
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
    }*/
    //return decodeURI(dc.substring(begin + prefix.length, end));
};


function updateImageBorders(component) {
    $(".big-background .bottom-left").css({ "margin-top": $(".game-plane").height() - 40 })
    $(".big-background .bottom-right").css({ "margin-top": $(".game-plane").height() - 40 })
    $(".big-background .bottom-left").css({ "visibility": "visible" })
    $(".big-background .bottom-right").css({ "visibility": "visible" })
    document.cookie = component + "=true";
};

function updateCharacterInfo(gold, exp, reqExp, level) {
    if (exp != $("#character-exp").text() && exp != null) {
        $("#character-exp").fadeOut(300, function () {
            $(this).text(exp.toString()).fadeIn(300);
        });
    }
    if (gold != $("#character-gold").text() && gold != null) {
        $("#character-gold").fadeOut(300, function () {
            $(this).text(gold.toString()).fadeIn(300);
        })
    }
    if (reqExp != $("#character-reqExp").text() && reqExp != null) {
        $("#character-reqExp").fadeOut(300, function () {
            $(this).text(reqExp.toString()).fadeIn(300);
        })
    }
    if (level != $("#character-level").text() && level != null) {
        $("#character-level").fadeOut(300, function () {
            $(this).text(level.toString()).fadeIn(300);
        })
    }
};