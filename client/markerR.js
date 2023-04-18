const map = require("./src/assets/map/markers.json");
const fs = require('fs');


const baseRC = map.RainCollector.init

let dataRC = []

for (let i = 1; i <= 100; i++) {
    let toAdd = baseRC.replace(
        `M 0 0 L 1 9 Q 1.15 10 2 10 L 7 10 Q 7.991 10.002 8 9 L 12 9 A 1 1 0 0 0 13 10 L 18 10 Q 18.85 10 19 9 L 20 0 L 11 0 L 12 8.5 L 8.001 8.5 L 9 0`,
        `M ${(i * 1) / 100} ${(i * 9) / 100} L 1 9 Q 1.15 10 2 10 L 7 10 Q 7.991 10.002 8 9 L 12 9 A 1 1 0 0 0 13 10 L 18 10 Q 18.85 10 19 9 L ${20 - ((i * 1) / 100)} ${(i * 9) / 100} L ${((i * 1) / 100) + 11} ${(i * 9) / 100} L 12 8.5 L 8.001 8.5 L ${9 - ((i * 1) / 100)} ${(i * 9) / 100}`
    )
    dataRC.push(toAdd)

    if (i == 80)
        console.log(toAdd)
}
map.RainCollector.data = dataRC

//console.log(JSON.stringify(map))
fs.writeFile('./src/assets/map/markers.json', JSON.stringify(map), err => {
    if (err) {
        console.error(err);
    }
});