import React, { Component } from 'react';

import FileUpload from './FileUpload';

import { ResponsiveLine } from '@nivo/line'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            result: []
        };
        this.setData = this.setData.bind(this);
    }

    setData = (data) => {
        console.log(data);
        if (!Array.isArray(data)) {
            this.setState({ result: [data] }, function () {
                console.log(this.state.result);
            });
        } else {
            this.setState({ result: data }, function () {
                console.log(this.state.result);
            });
        }
    }

    render() {
        return (
            <div>
                <FileUpload result={this.setData} />
                <div style={{ height: 500 }}>
                    <ResponsiveLine
                        data={this.state.result}
                        margin={{ top: 50, right: 160, bottom: 50, left: 60 }}
                        xScale={{ type: 'linear', stacked: true, min: 0, max: 4.5 }}
                        yScale={{ type: 'linear', stacked: true, min: -40, max: 40 }}
                        yFormat=" >-.2f"
                        curve="monotoneX"
                        axisTop={null}
                        axisRight={null}
                        axisBottom={{
                            tickValues: [
                                0,
                                0.5,
                                1.0,
                                1.5,
                                2.0,
                                2.5,
                                3.0,
                                3.5,
                                4.0,
                                4.5
                            ],
                            tickSize: 0.1,
                            tickPadding: 0.1,
                            tickRotation: 0,
                            format: '.2f',
                            legend: 'tempo',
                            legendOffset: 36,
                            legendPosition: 'middle'
                        }}
                        axisLeft={{
                            tickValues: [
                                -40,
                                -30,
                                -20,
                                -10,
                                0,
                                10,
                                20,
                                30,
                                40
                            ],
                            tickSize: 10,
                            tickPadding: 10,
                            tickRotation: 0,
                            format: '.2s',
                            legend: 'V',
                            legendOffset: -40,
                            legendPosition: 'middle'
                        }}
                        enableGridX={true}
                        colors={{ scheme: 'spectral' }}
                        lineWidth={1}
                        enablePoints={false}
                        pointSize={4}
                        pointColor={{ theme: 'background' }}
                        pointBorderWidth={1}
                        pointBorderColor={{ from: 'serieColor' }}
                        pointLabelYOffset={-12}
                        useMesh={true}
                        gridXValues={[0, 20, 40, 60, 80, 100, 120]}
                        gridYValues={[0, 500, 1000, 1500, 2000, 2500]}
                        legends={[
                            {
                                anchor: 'bottom-right',
                                direction: 'column',
                                justify: false,
                                translateX: 140,
                                translateY: 0,
                                itemsSpacing: 2,
                                itemDirection: 'left-to-right',
                                itemWidth: 80,
                                itemHeight: 12,
                                itemOpacity: 0.75,
                                symbolSize: 12,
                                symbolShape: 'circle',
                                symbolBorderColor: 'rgba(0, 0, 0, .5)',
                                effects: [
                                    {
                                        on: 'hover',
                                        style: {
                                            itemBackground: 'rgba(0, 0, 0, .03)',
                                            itemOpacity: 1
                                        }
                                    }
                                ]
                            }
                        ]}
                    />
                </div>
            </div>
        );
    }
}
