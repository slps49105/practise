$(function () {
    $('.hidden').hide();
    $('.day').show();
    $(".content-main").addClass("dy");
    $(".add").addClass("dyadd");

    $('.daily').on("click", function () {
        contentSel("dy", "wy my iy", "dyadd", "wyadd myadd iyadd", ".day", ".D")
    });
    $('.weekly').on("click", function () {
        contentSel("wy", "dy my iy", "wyadd", "dyadd myadd iyadd", ".week", ".W")
    });
    $('.monthly').on("click", function () {
        contentSel("my", "dy wy iy", "myadd", "dyadd wyadd iyadd", ".month", ".M")
    });
    $('.irregularly').on("click", function () {
        contentSel("iy", "dy wy my", "iyadd", "dyadd wyadd myadd", ".irregular", ".I")
    });

    $(".content-main").on('click', ".dyadd, .wyadd, .myadd", function () {
        const className = $(this).attr("class").split(' ')[1]; // this為被點擊的對象,attr("class")取得該對象的所有class,split把每個class用空格做區隔,[1]選擇第二個class(0為第一個，以此類推)
        const target = className.slice(0, 2); // 取得className前兩個字母(0到2之前)
        const section = target === "dy" ? ".day" : target === "wy" ? ".week" : ".month";
        const section2 = target === "dy" ? "day" : target === "wy" ? "week" : "month";
        addSel(`.${className}`, section, section2);
    });

    $(".content-main").on('click', ".iyadd", function () {
        $(".iyadd").hide();
        $(".irregular").append(`
            <div class="irregular">
                <input type="text" class="insert" name="insert">
                <input type="datetime-local" class="deadline" name="deadline">
                <input type="submit" class="sure" value="確定">
                <input type="submit" class="cancel2" name="cancel" value="取消">
            </div>  
        `)
    })

    $(".day, .week, .month").on('click', ".sure", function () {
        const parentClass = $(this).closest("div").attr("class"); // 取得父元素的 class
        const insertType = parentClass.charAt(0).toUpperCase() + "insert"; // 取父元素的首字母並加上 "insert"
        sendSel(insertType);
    });

    $(".irregular").on('click', ".sure", function () {
        $.ajax({
            type: "POST",
            url: "func/Iinsert.php",
            dataType: "json",
            data: {
                Iinsert: $(".insert").val(),
                deadline: $(".deadline").val()
            }
        });
    });

    $(".labels").on('click', ".cancel", function () {
        $(".tem, #dateup").remove();
        $(".update, .delete").show();
    });

    $("#addItemBtn").on("click", function () {
        $("#inputSection").show();
        $("#addItemBtn").hide();
    });

    // 點擊 "Cancel" 隱藏輸入框和按鈕
    $("#cancelBtn").on("click", function () {
        $("#inputSection").hide();
        $("#itemInput").val(""); // 清空輸入框
        $("#addItemBtn").show();
    });

    // 點擊 "Confirm" 發送 AJAX 請求
    $("#confirmBtn").on("click", function () {
        var itemName = $("#itemInput").val();
        console.log(itemName);
        $.ajax({
            type: "POST",
            url: "/Home/Create",
            contentType: "application/json",
            data: JSON.stringify({
                ItemName: itemName
            }),
            success: function (response) {
                console.log("Data sent successfully:", response);
                location.reload();
            },
            error: function (xhr) {
                console.error("Error occurred:", xhr.responseText);
            },
        });
    });

    $(document).on("click", ".update", function () {
        var dailyId = $(this).data("daily-id"); // 獲取被點擊項目的 ID
        var $item = $(this).closest(".item"); // 獲取當前的 item 容器

        // 檢查是否已經有 input 框，避免重複生成
        if ($item.find(".edit-input").length > 0) return;

        // 獲取原始值
        var originalValue = $item.find("label").text().trim();

        // 動態生成 input 和按鈕
        var editHtml = `
        <div class="edit-container">
            <input type="text" class="edit-input" value="${originalValue}" />
            <button class="confirm-btn" data-daily-id="${dailyId}">Confirm</button>
            <button class="cancel-btn">Cancel</button>
        </div>
    `;

        // 隱藏原本的內容，並插入編輯表單
        $item.find("label, input, a").hide();
        $item.append(editHtml);
    });

    // 點擊 Confirm 按鈕
    $(document).on("click", ".confirm-btn", function () {
        var dailyId = $(this).data("daily-id");
        var $item = $(this).closest(".item");
        var newValue = $item.find(".edit-input").val().trim();

        if (!newValue) {
            alert("Value cannot be empty.");
            return;
        }

        // 發送 AJAX 更新資料
        $.ajax({
            type: "POST",
            url: "/Home/Update", // 假設此為後端的更新方法
            contentType: "application/json",
            data: JSON.stringify({
                Id: dailyId,
                ItemName: newValue
            }),
            success: function () {
                // 更新成功後顯示新值
                $item.find("label").text(newValue).show();
                $item.find("input, a").show();
                $item.find(".edit-container").remove();
            },
            error: function () {
                alert("Failed to update.");
            },
        });
    });

    // 點擊 Cancel 按鈕
    $(document).on("click", ".cancel-btn", function () {
        var $item = $(this).closest(".item");
        $item.find("label, input, a").show();
        $item.find(".edit-container").remove();
    });

    $(window).on("scroll", function () {
        var scrollPosition = $(window).scrollTop();
        if (scrollPosition > 200) {
            $('.header').addClass('white').removeClass('vh3');
        } else {
            $('.header').addClass('vh3').removeClass('white');
        }
    });

    setInterval(() => {
        const now = new Date();
        document.getElementById('clock').textContent = now.toLocaleTimeString();
        if (now.getHours() === 0 && now.getMinutes() === 0 && now.getSeconds() === 0) {
            location.reload();
        }
    }, 1000);
})
