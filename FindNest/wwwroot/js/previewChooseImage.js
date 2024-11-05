function previewImages(inputFiles, displayTarget, indexInputName, indexClassName) {
    const files = inputFiles.files;

    $.each(files, function (index, file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imgContainer = $('<div class="sortable-img position-relative"></div>');
            const img = $('<img>').attr('src', e.target.result).addClass('h-100 rounded');

            // Tạo ô input để lưu trữ tệp
            const fileInput = $('<input>').attr({
                type: 'file',
                name: inputFiles.name, 
                style: 'display: none;' 
            });

            // Tạo một đối tượng File để gán vào ô input
            const dataTransfer = new DataTransfer();
            dataTransfer.items.add(file);
            fileInput[0].files = dataTransfer.files; 

            // Tạo ô input hidden để lưu index
            const indexInput = $('<input>').attr({
                type: 'hidden',
                name: indexInputName, 
                value: '', 
                class: 'FileIndex'
            });

            // Tạo nút xóa
            const removeBtn = $('<button class="btn btn-dark opacity-75 rounded-circle position-absolute py-0 px-2 top-0 end-0">x</button>');

            // Thêm sự kiện xóa cho nút
            removeBtn.on('click', function () {
                imgContainer.remove(); 
                updateIndexInputs(); 
            });

            // Thêm các phần tử vào imgContainer
            imgContainer.append(img).append(removeBtn).append(fileInput).append(indexInput); 
            $(displayTarget).append(imgContainer); 

            // Cập nhật chỉ số cho ô input hidden
            updateIndexInputs();
        };
        reader.readAsDataURL(file);
    });

    // Kích hoạt khả năng kéo và thả
    $(displayTarget).sortable({
        items: '.sortable-img', 
        cursor: 'move', 
        placeholder: 'sortable-placeholder', 
        update: function (event, ui) {
            updateIndexInputs();
        }
    });

    // Hàm cập nhật chỉ số cho các ô input hidden
    function updateIndexInputs() {
        $(displayTarget).children().each(function () {
            const index = $(this).index();
            console.log(indexInputName)
            console.log(index)
            $(this).find('input[type="hidden"].' + indexClassName).val(index);
        });
    }
}

