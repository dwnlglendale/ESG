$(document).on('click', '[data-download-id]', function (e) {

    var downloadId = $(this).attr('data-download-id');
    console.log("downloadId-->>>", downloadId)


    //show_loader();

    //displaySuccessToast("Download Success", "File <b>" + downloadId + "</b> Was Downloaded Successfully");

    var fileName = downloadId;
    var filePath = document.location.origin + "/holdFiles/" + fileName; 

    downloadURI(filePath,fileName);
              
});


//process the download here
function downloadURI(filePath,fileName) {
    console.log(filePath);
    var link = document.createElement('a');
    link.href = filePath;
    link.download = fileName;
    link.click();
}