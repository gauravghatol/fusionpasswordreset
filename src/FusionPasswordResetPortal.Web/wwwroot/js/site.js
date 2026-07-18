$(function () {
    $("form").on("submit", function () {
        const button = $(this).find("button[type='submit']");
        button.prop("disabled", true);
        button.text("Processing...");
    });
});
