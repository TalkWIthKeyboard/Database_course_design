$(
    function () {
        var uploader = Qiniu.uploader({
            runtimes: 'html5,flash,html4',
            browse_button: 'pickfiles',//上传按钮的ID
            container: 'btn-uploader',//上传按钮的上级元素ID
            drop_element: 'btn-uploader',
            max_file_size: '100mb',//最大文件限制
            flash_swf_url: '/static/js/plupload/Moxie.swf',
            dragdrop: false,
            chunk_size: '4mb',//分块大小
            //uptoken_url: '/qiniu-token/',//设置请求qiniu-token的url
            //Ajax请求upToken的Url，**强烈建议设置**（服务端提供）
            uptoken: "PTvRc8G0vxeyXEfUp1ZSEbZOzpKwmewIB_Uvm1BG:SMJ6t3-Q3CgusixV4JJ8FUOF46I=:eyJzY29wZSI6ImZpbGVtYW5hZ2UiLCJkZWFkbGluZSI6MTQ2OTE0MjI0NywiaW5zZXJ0T25seSI6MCwiZGV0ZWN0TWltZSI6MCwiZnNpemVMaW1pdCI6MCwiZnNpemVNaW4iOjAsImNhbGxiYWNrRmV0Y2hLZXkiOjB9",
            //若未指定uptoken_url,则必须指定 uptoken ,uptoken由其他程序生成
            unique_names: true,
            // 默认 false，key为文件名。若开启该选项，SDK会为每个文件自动生成key（文件名）
            save_key: true,
            // 默认 false。若在服务端生成uptoken的上传策略中指定了 `sava_key`，则开启，SDK在前端将不对key进行任何处理
            domain: 'http://o8efk558i.bkt.clouddn.com/',//自己的七牛云存储空间域名
            multi_selection: false,//是否允许同时选择多文件
            //文件类型过滤，这里限制为图片类型
            filters: {
                mime_types: [
                    { title: "Image files", extensions: "jpg,jpeg,gif,png" }
                ]
            },
            auto_start: true,
            init: {
                'FilesAdded': function (up, files) {
                    //do something
                },
                'BeforeUpload': function (up, file) {
                    //do something
                },
                'UploadProgress': function (up, file) {
                    //可以在这里控制上传进度的显示
                    //可参考七牛的例子
                },
                'UploadComplete': function () {
                    //do something
                },
                'FileUploaded': function (up, file, info) {
                    //每个文件上传成功后,处理相关的事情
                    //其中 info 是文件上传成功后，服务端返回的json，形式如
                    //{
                    //  "hash": "Fh8xVqod2MQ1mocfI4S4KpRL6D98",
                    //  "key": "gogopher.jpg"
                    //}
                    var domain = up.getOption('domain');
                    var res = eval('(' + info + ')');
                    var sourceLink = domain + res.key;//获取上传文件的链接地址

                    var json = JSON.parse(info);
                    var hash = json['hash'];
                    StandardPost('/Home/', {'fileHash': hash});
                    //do something
                },
                'Error': function (up, err, errTip) {
                    alert(errTip);
                },
                'Key': function (up, file) {
                    //当save_key和unique_names设为false时，该方法将被调用
                    var key = "";
                    $.ajax({
                        url: '/qiniu-token/get-key/',
                        type: 'GET',
                        async: false,//这里应设置为同步的方式
                        success: function (data) {
                            var ext = Qiniu.getFileExtension(file.name);
                            key = data + '.' + ext;
                        },
                        cache: false
                    });
                    return key;
                }
            }
        });
    }
)