
// # MapRefiner.js
// Script to refine the garden to realstic scaled map for client

const map = require("./rawMap.json");
const fs = require('fs');

var rMap = {
    "type": "FeatureCollection",
    "features": []
}

var baseFeature = {
    "type": "Feature",
    "geometry": {
        "type": "Polygon",
        "coordinates": []
    },
    "properties": {}
}

const latitudeMultiplier = 3.0;  //  |
const longitudeMultiplier = 5.0; // ---
const circularOffset = 1.5; // simple triangular calculation for approximite curve clac

map.forEach(area => {
    var feature = JSON.parse(JSON.stringify(baseFeature))
    var currentFeatures = []
    feature.properties.id = area.properties.id
    feature.properties.name = area.properties.name
    feature.properties.color = area.properties.color

    const points = area.measurements
    const position = area.position

    const multipliedPoints = CalcMultipliePoints(points);
    const boundaries = CalcBoundaries(multipliedPoints);
    const offsetPoints = CalcOffset(position, multipliedPoints, boundaries);

    feature.geometry.coordinates.push(offsetPoints)

    currentFeatures.push(feature)
    rMap.features.push(feature)
});

console.log("___");
console.log(JSON.stringify(rMap));
fs.writeFile('./public/map.json', JSON.stringify(rMap), err => {
    if (err) {
        console.error(err);
    }
});



// Multiplie the points to fit in a real coord system
function CalcMultipliePoints(points) {
    var multipliedPoints = []
    points.forEach(point => {
        multipliedPoints.push([
            point[0] * longitudeMultiplier,
            (point[1] * latitudeMultiplier) ^ circularOffset
        ])
    });
    return multipliedPoints
}

// Applay the Offset to center
function CalcBoundaries(points, alignment = ["top", "left"]) {
    var X = 0;
    var Y = 0;

    switch (alignment[0]) {
        case "top": points.forEach(point => {
            if (point[1] > Y) Y = point[1]
        });
            break;

        case "bottom": points.forEach(point => {
            if (point[1] < Y) Y = point[1]
        });
            break;
    }

    switch (alignment[1]) {
        case "left": points.forEach(point => {
            if (point[0] > X) X = point[0]
        });
            break;
    }

    return [X, Y]
}

function CalcOffset(position, points, boundaries) {
    var offsetPoints = []

    if (position) {
        const parent = rMap.features.find(f => f.properties.id === position.parent)
        if (!parent) return BaseOffset(points, boundaries)

        const parentPoints = parent.geometry.coordinates
        const parentBoundaries = CalcBoundaries(parentPoints[0], position.alignment)

        var offsetPoints = []
        points.forEach(point => {
            offsetPoints.push([
                point[0] - parentBoundaries[0] + position.margin[0],
                point[1] + parentBoundaries[1] + position.margin[1]
            ])
        });

    }
    else return BaseOffset(points, boundaries)

    return offsetPoints
}

function BaseOffset(points, boundaries) {
    var offsetPoints = []
    points.forEach(point => {
        offsetPoints.push([
            point[0] - boundaries[0] / 2,
            point[1] - boundaries[1] / 2
        ])
    });
    return offsetPoints;
}