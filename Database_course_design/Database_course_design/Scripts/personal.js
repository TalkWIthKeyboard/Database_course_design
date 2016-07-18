$(
    function () {
        // 按钮点击效果
        var $buttons = $('#social button'),
            $followButton = $('#follow'),
            $unfollowButton = $('#unfollow');

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
            });
        });

        // 画图表
        var myChart = echarts.init(document.getElementById('chart'));

        var dataBJ = [
        [0, 30, null],
        [1, 15, 30, "Moderately polluted"],
        [2, 10, 11, "Excellent"],
        [3, 0, 1, "what"]
        [4, 15, 30, "Moderately polluted"],
        [5, 10, 11, "Excellent"],
        [6, 0, 1, "what"]
        [7, 15, 30, "Moderately polluted"],
        [8, 10, 11, "Excellent"],
        [9, 0, 1, "what"]
        [10, 15, 30, "Moderately polluted"],
        [11, 10, 11, "Excellent"],
        [12, 0, 1, "what"]
        ];

        var schema = [
            { name: 'date', index: 0, text: 'Day' },
            { name: 'xx', index: 1, text: 'xx' },
            { name: 'xx', index: 2, text: 'xx' },
            { name: 'xx', index: 3, text: 'xx' },
            { name: 'xx', index: 4, text: 'xx' },
            { name: 'xx', index: 5, text: 'xx' },
            { name: 'xx', index: 6, text: 'xx' }
        ];


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
                        + value[0] + 'Day'
                        + '</div>'
                        + schema[1].text + '：' + value[3] + '<br>'
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
                        color: '#fff'
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
    }
)
