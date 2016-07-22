$(
    function () {
        var uploader = Qiniu.uploader({
            runtimes: 'html5,flash,html4',
            browse_button: 'pickfiles',//�ϴ���ť��ID
            container: 'btn-uploader',//�ϴ���ť���ϼ�Ԫ��ID
            drop_element: 'btn-uploader',
            max_file_size: '100mb',//����ļ�����
            flash_swf_url: '/static/js/plupload/Moxie.swf',
            dragdrop: false,
            chunk_size: '4mb',//�ֿ��С
            //uptoken_url: '/qiniu-token/',//��������qiniu-token��url
            //Ajax����upToken��Url��**ǿ�ҽ�������**��������ṩ��
            uptoken: "PTvRc8G0vxeyXEfUp1ZSEbZOzpKwmewIB_Uvm1BG:SMJ6t3-Q3CgusixV4JJ8FUOF46I=:eyJzY29wZSI6ImZpbGVtYW5hZ2UiLCJkZWFkbGluZSI6MTQ2OTE0MjI0NywiaW5zZXJ0T25seSI6MCwiZGV0ZWN0TWltZSI6MCwiZnNpemVMaW1pdCI6MCwiZnNpemVNaW4iOjAsImNhbGxiYWNrRmV0Y2hLZXkiOjB9",
            //��δָ��uptoken_url,�����ָ�� uptoken ,uptoken��������������
            unique_names: true,
            // Ĭ�� false��keyΪ�ļ�������������ѡ�SDK��Ϊÿ���ļ��Զ�����key���ļ�����
            save_key: true,
            // Ĭ�� false�����ڷ��������uptoken���ϴ�������ָ���� `sava_key`��������SDK��ǰ�˽�����key�����κδ���
            domain: 'http://o8efk558i.bkt.clouddn.com/',//�Լ�����ţ�ƴ洢�ռ�����
            multi_selection: false,//�Ƿ�����ͬʱѡ����ļ�
            //�ļ����͹��ˣ���������ΪͼƬ����
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
                    //��������������ϴ����ȵ���ʾ
                    //�ɲο���ţ������
                },
                'UploadComplete': function () {
                    //do something
                },
                'FileUploaded': function (up, file, info) {
                    //ÿ���ļ��ϴ��ɹ���,������ص�����
                    //���� info ���ļ��ϴ��ɹ��󣬷���˷��ص�json����ʽ��
                    //{
                    //  "hash": "Fh8xVqod2MQ1mocfI4S4KpRL6D98",
                    //  "key": "gogopher.jpg"
                    //}
                    var domain = up.getOption('domain');
                    var res = eval('(' + info + ')');
                    var sourceLink = domain + res.key;//��ȡ�ϴ��ļ������ӵ�ַ

                    var json = JSON.parse(info);
                    var hash = json['hash'];
                    StandardPost('/Home/', {'fileHash': hash});
                    //do something
                },
                'Error': function (up, err, errTip) {
                    alert(errTip);
                },
                'Key': function (up, file) {
                    //��save_key��unique_names��Ϊfalseʱ���÷�����������
                    var key = "";
                    $.ajax({
                        url: '/qiniu-token/get-key/',
                        type: 'GET',
                        async: false,//����Ӧ����Ϊͬ���ķ�ʽ
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