$(
    function () {

        // 按钮点击效果
        var $buttons = $('#social button'),
            $followButton = $('.follow'),
            $unfollowButton = $('.unfollow');

        $buttons.each(function(index, el) {
            $(el).click(function(event) {

                $(el).animate({top: '-1'}, 200)
                    .css({'z-index': '0', 'cursor': 'auto'})
                    .animate({top: '19', right: '5'}, 200, function() {
                        $(this).attr('disabled', true);
                    })
                $($buttons[1 - index]).animate({top: '35'}, 200)
                    .css({'z-index': '1', 'cursor': 'pointer'})
                    .animate({top: '15', right: '10'}, 200, function() {
                        $(this).attr('disabled', false);
                    });

                var followed = true;
                if ($($buttons[1 - index]).attr('class').split(' ')[0] === 'follow') {
                    followed = false;
                }

                $.post('/Home/follow', { 'followed': followed }, function (data) {
                });
            });
        });

        // 点击选项卡切换概览，详情和留言
        var $navLink = $('#repository nav a');
        $navLink.each(function(index, el) {
            $(el).click(function (event) {
                var type = '';
                if ($(el).attr('id') === 'navLink1') {
                    type = '1';
                } else if ($(el).attr('id') === 'navLink2') {
                    type = '2';
                } else {
                    type = '3';
                }
                
                StandardPost('/Home/Choose', {'type': type});
            })
        });

        // 点击弹窗编辑资料
        $('#edit').click(function(event) {
            $('#fill').show(0);
            $('#popup').addClass('show');
        });

        $('#popup form button').click(function(event) {
            $('#popup').removeClass('show');
            $('#fill').delay(200).hide(0);
        })

        // 切换弹窗表单
        var $formNavLink = $('#popup nav a');
        $formNavLink.each(function(index, el) {
            $(el).click(function(event) {
                $(el).addClass('selected');
                $($formNavLink[1 - index]).removeClass('selected');

                if (index === 0) {
                    $('#popup').css('height', '540');
                    $('#basicForm').css('left', '+=404');
                    $('#passwordForm').css({'left': '+=404'});
                } else {
                    $('#popup').css('height', '400');
                    $('#basicForm').css('left', '-=404');
                    $('#passwordForm').css({'left': '-=404'});
                }
            });
        });

        // 修改账号信息
        $('#basicForm .submitDiv input').click(function () {
            var username = $('#basicForm input[name="username"]').val();
            var signature = $('#basicForm textarea').val();
            var email = $('#basicForm input[name="email"]').val();
            var address = $('#basicForm input[name="address"]').val();

            StandardPost('/Home/PersonalInfo', {
                'username': username, 'signature': signature,
                'email': email, 'address': address
            });
        });

        // 修改密码
        $('#passwordForm .submitDiv input').click(function () {
            var oldPassword = $('#passwordForm input[name="password"]').val();
            var newPassword = $('#passwordForm input[name="newPassword"]').val();
            var repeatPassword = $('#passwordForm input[name="repeatPassword"]').val();

            StandardPost('/Home/ChangeBasicInfo', { 'oldPassword': oldPassword, 'newPassword': newPassword, 'repeatPassword': repeatPassword });
        })

        // 评论框输入
        $('#messageInput form :button').click(function () {
            var commit = $('#messageInput form textarea').val();
            /*$.post('/Home/', { 'commit': commit });*/
            var head = $('#content .head img').attr('src');
            var name = $('#content p').html();
            $('#messageBoard').prepend('<li class="message"><div><a href="#"><img src="' + head + '"/></a></div><div><span>' +
                name + '</span><p>' + commit + '</p><span>r32r32</span></div></li>');
        })

        /*统计*/
        $.ajax({
            url: "/Home/statistics",
            type: "POST",
            dataType: "json",
            data: {},
            success: function (data) {
                // 画图表
                var myChart = echarts.init(document.getElementById('chart'));
                var dataBJ = [
                [0, 30, null],
                ];
                var schema = [
                    { name: 'date', index: 0, text: 'Day' },
                ];

                for (var i = 1 ; i < data.length; ++i) {
                    var count = data[i].Count;
                    var heat = "";
                    if (count >= 0 && count <= 10) heat += "贡献平平";
                    else if (count <= 20) heat += "学霸练就中";
                    else heat += "我猜你是老师吧"

                    dataBJ.push([i, count, count, heat]);
                    for (var j = 0; j < count; ++j) {
                        var label1 = data[i].OpList[j].OPERATION;
                        var label2 = data[i].OpList[j].TARGET_USER_NAME;
                        var label3 = data[i].OpList[j].TARGET_REPOSITORY_NAME;
                        schema.push({ name: label1, index: label2, text: label3 });
                    }
                }


                var itemStyle = {
                    normal: {
                        opacity: 0.8,
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowOffsetY: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                };

                var option = {
                    backgroundColor: '#F3F3F3',
                    color: [
                        '#75E7C2'
                    ],
                    grid: {
                        x: 50,
                        x2: 150,
                        y: 45,
                        y2: '10%'
                    },
                    tooltip: {
                        padding: 10,
                        backgroundColor: '#03ACDC',
                        borderColor: '#4DFBB6',
                        borderWidth: 1,
                        formatter: function (obj) {
                            var value = obj.value;
                            return '<div style="border-bottom: 1px solid rgba(255,255,255,.3); font-size: 18px;padding-bottom: 7px;margin-bottom: 7px">'
                                + value[0] + 'Day' + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + value[3]
                                + '</div>'
                                + "1." + schema[1].name + '<br>' + schema[1].index + schema[1].text + '<br>'
                        }
                    },
                    xAxis: {
                        type: 'value',
                        name: 'Date',
                        nameGap: 5,
                        nameTextStyle: {
                            color: '#999999',
                            fontSize: 14
                        },
                        max: 31,
                        splitLine: {
                            show: false
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#999999'
                            }
                        },
                        axisTick: {
                            lineStyle: {
                                color: '#999999'
                            }
                        },
                        axisLabel: {
                            formatter: '{value}',
                            textStyle: {
                                color: '#999999'
                            }
                        }
                    },
                    yAxis: {
                        type: 'value',
                        name: 'ContriNums',
                        nameLocation: 'end',
                        nameGap: 20,
                        nameTextStyle: {
                            color: '#999999',
                            fontSize: 16
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#777'
                            }
                        },
                        axisTick: {
                            lineStyle: {
                                color: '#777'
                            }
                        },
                        splitLine: {
                            show: false
                        },
                        axisLabel: {
                            textStyle: {
                                color: '#999999'
                            }
                        }
                    },
                    visualMap: [
                        {
                            right: '5%',
                            bottom: '5%',
                            min: 0,
                            max: 30,
                            itemHeight: 120,
                            precision: 0.1,
                            text: ['热度'],
                            textGap: 30,
                            textStyle: {
                                color: '#75E7C2'
                            },
                            inRange: {
                                symbolSize: [10, 30],
                                colorLightness: [1, 0.5]
                            },
                            controller: {
                                inRange: {
                                    color: ['#75E7C2']
                                },
                                outOfRange: {
                                    color: ['#75E7C2']
                                }
                            }
                        }
                    ],
                    series: [
                        {
                            type: 'scatter',
                            itemStyle: itemStyle,
                            data: dataBJ
                        }
                    ]
                };

                myChart.setOption(option);

            },
            error: function () {
                alert("页面出错了");
            }
        });

    }
)
