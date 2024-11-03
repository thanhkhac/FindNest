    function previewImages(inputFiles, displayTarget, indexInputName) {
    const files = inputFiles.files;

    $.each(files, function(index, file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            const imgContainer = $('<div class="sortable-img position-relative"></div>');
            const img = $('<img>').attr('src', e.target.result).addClass('h-100 rounded');

            // Tạo ô input để lưu trữ tệp
            const fileInput = $('<input>').attr({
                type: 'file',
                name: inputFiles.name, // Đặt tên cho ô input để gửi tệp trong form
                style: 'display: none;' // Ẩn ô input
            });

            // Tạo một đối tượng File để gán vào ô input
            const dataTransfer = new DataTransfer();
            dataTransfer.items.add(file);
            fileInput[0].files = dataTransfer.files; // Gán tệp vào ô input

            // Tạo ô input hidden để lưu chỉ số
            const indexInput = $('<input>').attr({
                type: 'hidden',
                name: indexInputName, // Tên để gửi chỉ số
                value: '', // Giá trị sẽ được cập nhật sau
            });

            // Tạo nút xóa
            const removeBtn = $('<button class="btn btn-dark opacity-75 rounded-circle position-absolute py-0 px-2 top-0 end-0">x</button>');

            // Thêm sự kiện xóa cho nút
            removeBtn.on('click', function() {
                imgContainer.remove(); // Xóa hình ảnh và ô input
                updateIndexInputs(); // Cập nhật lại chỉ số sau khi xóa
            });

            // Thêm các phần tử vào imgContainer
            imgContainer.append(img).append(removeBtn).append(fileInput).append(indexInput); // Thêm cả ô input và ô input hidden vào imgContainer
            $(displayTarget).append(imgContainer); // Thêm imgContainer vào displayTarget

            // Cập nhật chỉ số cho ô input hidden
            updateIndexInputs();
        };
        reader.readAsDataURL(file);
    });

    // Kích hoạt khả năng kéo và thả
    $(displayTarget).sortable({
        items: '.sortable-img', // Các phần tử có thể kéo
        cursor: 'move', // Con trỏ khi kéo
        placeholder: 'sortable-placeholder', // Placeholder khi kéo
        update: function(event, ui) {
            // Cập nhật chỉ số cho các ô input hidden sau khi thay đổi thứ tự
            updateIndexInputs();
        }
    });

    // Hàm cập nhật chỉ số cho các ô input hidden
    function updateIndexInputs() {
        $(displayTarget).children().each(function() {
            // Lấy chỉ số của phần tử hiện tại so với tất cả các phần tử con
            const index = $(this).index();
            console.log(indexInputName)
            console.log(index)
            $(this).find('input[type="hidden"][name="' + indexInputName + '"]').val(index);
        });
    }
}
