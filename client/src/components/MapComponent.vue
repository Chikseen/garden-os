<template>
  <div id="map"></div>
</template>

<script>
import GeoJSON from "ol/format/GeoJSON.js";
import Map from "ol/Map.js";
import VectorLayer from "ol/layer/Vector.js";
import VectorSource from "ol/source/Vector.js";
import View from "ol/View.js";
import { Fill, Icon, Stroke, Style } from "ol/style.js";
import Feature from "ol/Feature";
import { Point } from "ol/geom";

export default {
  components: {},
  data() {
    return {};
  },
  methods: {},
  mounted() {
    const style = new Style({
      fill: new Fill({
        color: "#eeeeee",
      }),
    });

    //
    var iconFeature = new Feature({
      geometry: new Point([0, 0]),
    });

    var svg =
      '<svg viewBox="-1194.11 -7880.94 13300 11380" width="500" height="500" version="1.1" xmlns="http://www.w3.org/2000/svg" fill="red">' +
      '<path fill="red" d="M 0 0 A 1 1 0 0 0 1188 3371 A 1 1 0 0 0 2508 -1 L 2535 -6537 A 1 1 0 0 0 -82 -6605" />' +
      "</svg>";

    var SVGstyle = new Style({
      image: new Icon({
        opacity: 0.75,
        src: "data:image/svg+xml;utf8," + svg,
        scale: 0.25,
      }),
    });

    iconFeature.setStyle(SVGstyle);

    var vectorSource = new VectorSource({
      features: [iconFeature],
    });

    var svgLayer = new VectorLayer({
      source: vectorSource,
    });
    //

    const vectorLayer = new VectorLayer({
      background: "#1a2b39",
      source: new VectorSource({
        url: "/map.json",
        format: new GeoJSON(),
      }),
      style: function (feature) {
        const color = feature.get("COLOR") || "#eeeeee";
        style.getFill().setColor(color);
        return style;
      },
    });

    const map = new Map({
      layers: [vectorLayer, svgLayer],
      target: "map",
      view: new View({
        center: [0, 0],
        zoom: 3,
      }),
    });

    const featureOverlay = new VectorLayer({
      source: new VectorSource(),
      map: map,
      style: new Style({
        stroke: new Stroke({
          color: "rgba(255, 255, 255, 0.7)",
          width: 5,
        }),
      }),
    });

    let highlight;
    const displayFeatureInfo = function (pixel) {
      const feature = map.forEachFeatureAtPixel(pixel, function (feature) {
        return feature;
      });

      if (feature !== highlight) {
        if (highlight) {
          featureOverlay.getSource().removeFeature(highlight);
        }
        if (feature) {
          featureOverlay.getSource().addFeature(feature);
        }
        highlight = feature;
      }
    };

    map.on("pointermove", function (evt) {
      if (evt.dragging) {
        return;
      }
      const pixel = map.getEventPixel(evt.originalEvent);
      displayFeatureInfo(pixel);
    });

    //map.on("click", (evt) => this.test(evt.target));
    map.on("postrender", function (event) {
      //console.log("hi", event);
      event;
      map.render();
      let dynamicStyle = {
        polygon: new Style({
          stroke: new Stroke({
            color: [255, 204, 0, 1],
            width: 10,
          }),
          fill: new Fill({
            color: [255, 0, 51, 0.4],
          }),
        }),
      };

      vectorLayer.setStyle((e) => {
        return dynamicStyle["polygon"];
      });
    });
  },
};
</script>

<style lang="scss">
#map {
  height: 100%;
  max-width: 100%;
}
</style>
