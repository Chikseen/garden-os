// # MapRefiner.js
// Script to refine the garden to realstic scaled map for client
const map = require("./rawMap.json");

const fs = require("fs");

var rMap = {
  type: "FeatureCollection",
  features: [],
};

var baseFeature = {
  type: "Feature",
  geometry: {
    type: "Polygon",
    coordinates: [],
  },
  properties: {},
};

let dMap = JSON.parse(JSON.stringify(rMap));

const latitudeMultiplier = 3.0; //  |
const longitudeMultiplier = 5.0; // ---
const circularOffset = 1.5; // simple triangular calculation for approximite curve clac

// Refine Main Map
map.main.forEach((area) => {
  var feature = JSON.parse(JSON.stringify(baseFeature));
  feature.properties.id = area.properties.id;
  feature.properties.name = area.properties.name;
  feature.properties.color = area.properties.color;

  var currentFeature = [];
  var coordinates = [];

  let totalArea = 0;
  area.measurements.forEach((pointsData) => {
    if (pointsData.props.type === "pattern") {
      // ToDo: Do some reapting patterns here
    }

    const points = pointsData.data;
    const position = area.position;
    const multipliedPoints = CalcMultipliePoints(points);
    const boundaries = CalcBoundaries(multipliedPoints);
    const offsetPoints = CalcOffset(position, multipliedPoints, boundaries);
    coordinates.push(offsetPoints);

    totalArea += CalcArea(points);
  });

  feature.geometry.coordinates = coordinates;
  feature.properties.area = Math.abs(totalArea);

  currentFeature.push(feature);
  rMap.features.push(feature);
});

fs.writeFile("./public/map.json", JSON.stringify(rMap), (err) => {
  if (err) {
    console.error(err);
  }
});

// Refine Detailed map
map.detailed.forEach((area) => {
  var feature = JSON.parse(JSON.stringify(baseFeature));
  feature.properties.id = area.properties.id;
  feature.properties.name = area.properties.name;
  feature.properties.color = area.properties.color;

  var currentFeature = [];
  var coordinates = [];

  let totalArea = 0;
  area.measurements.forEach((pointsData) => {
    if (pointsData.props.type === "pattern") {
      // ToDo: Do some reapting patterns here
    }

    const points = pointsData.data;
    const position = area.position;
    const multipliedPoints = CalcMultipliePoints(points);
    const boundaries = CalcBoundaries(multipliedPoints);
    const offsetPoints = CalcOffset(position, multipliedPoints, boundaries);
    coordinates.push(offsetPoints);

    totalArea += CalcArea(points);
  });

  feature.geometry.coordinates = coordinates;
  feature.properties.area = Math.abs(totalArea);

  currentFeature.push(feature);
  dMap.features.push(feature);

  if (feature.properties.id == "main") main = feature;
});

console.log("___");

fs.writeFile("./public/detailed.json", JSON.stringify(dMap), (err) => {
  if (err) {
    console.error(err);
  }
});

// Multiplie the points to fit in a real coord system
function CalcMultipliePoints(points) {
  var multipliedPoints = [];
  points.forEach((point) => {
    multipliedPoints.push([point[0] * longitudeMultiplier, (point[1] * latitudeMultiplier) ^ circularOffset]);
  });
  return multipliedPoints;
}

// Applay the Offset to center
function CalcBoundaries(points, alignment = ["top", "left"]) {
  var X = 0;
  var Y = 0;

  switch (alignment[0]) {
    case "top":
      points.forEach((point) => {
        if (point[1] > Y) Y = point[1];
      });
      break;

    case "bottom":
      points.forEach((point) => {
        if (point[1] < Y) Y = point[1];
      });
      break;
  }

  switch (alignment[1]) {
    case "left":
      points.forEach((point) => {
        if (point[0] > X) X = point[0];
      });
      break;
  }
  return [X, Y];
}

function CalcOffset(position, points, boundaries) {
  var offsetPoints = [];

  if (position) {
    const parent = rMap.features.find((f) => f.properties.id === position.parent);
    if (!parent) return BaseOffset(points, boundaries);

    const parentPoints = parent.geometry.coordinates;
    const parentBoundaries = CalcBoundaries(parentPoints[0], position.alignment);

    var offsetPoints = [];
    points.forEach((point) => {
      offsetPoints.push([point[0] - parentBoundaries[0] + position.margin[0] * longitudeMultiplier, point[1] + parentBoundaries[1] + position.margin[1] * latitudeMultiplier]);
    });
  } else return BaseOffset(points, boundaries);

  return offsetPoints;
}

function BaseOffset(points, boundaries) {
  var offsetPoints = [];
  points.forEach((point) => {
    offsetPoints.push([point[0] - boundaries[0] / 2, point[1] - boundaries[1] / 2]);
  });
  return offsetPoints;
}

function CalcArea(points) {
  let totalLength = 0;
  for (i = 0; i < points.length - 1; i++) {
    const height = (points[i][1] + points[i + 1][1]) / 2;
    const width = points[i + 1][0] - points[i][0];
    const area = height * width;
    totalLength += area;
  }
  return totalLength;
}
