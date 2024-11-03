function previewImage(inputFiles, displayTarget) {
    const files = inputFiles.files;
    if (files.length === 0) return; // Kiểm tra xem có file nào được chọn không

    const reader = new FileReader();
    $(displayTarget).empty(); // Xóa các phần tử con hiện tại trong displayTarget

    reader.onload = function (e) {
        const img = $('<img>').attr('src', e.target.result).attr('style', 'object-fit:cover').addClass('w-100');
        $(displayTarget).append(img);
    };

    reader.readAsDataURL(files[0]); // Đọc file đầu tiên
}