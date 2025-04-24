//CheckBox狀態選擇
function StatusSel(UrlSel, DataId, DataItemStatus) {
    $.ajax({
        type: "POST",
        url: UrlSel,
        contentType: "application/json",
        data: JSON.stringify({
            Id: DataId,
            ItemStatus: DataItemStatus,
        }),
        success: function (response) {
            console.log("Data sent successfully:", response);
        },
        error: function (xhr) {
            console.error("Error occurred:", xhr.responseText);
        },
    });
}

