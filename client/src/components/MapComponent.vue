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
import { Geometry, Point } from "ol/geom.js";

import _marker from "@/assets/map/markers.json";

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

    var svg = _marker.RainCollector.svg;

    const vectorLayer = new VectorLayer({
      background: "#1a2b39",
      source: new VectorSource({
        url: "/map.json",
        format: new GeoJSON(),
      }),
      style: function (feature) {
        const color = feature.get("color") || "#eeeeee";
        style.getFill().setColor(color);
        return style;
      },
    });

    var iconFeature = new Feature({
      type: "icon",
      geometry: new Point([0, 0]),
      name: "Null Island",
    });

    var markerStyle = new Style({
      image: new Icon({
        anchor: [0.25, 0],
        anchorXUnits: "fraction",
        anchorYUnits: "pixels",
        opacity: 0.5,
        width: 100,
        height: 100,
        //scale: 0.25
        offset: [0, 0],
        src: "data:image/svg+xml;utf8," + svg,
        geometry: new Geometry(new Point([1150, 1150])),
      }),
    });

    iconFeature.setStyle(markerStyle);

    var markerLayer = new VectorLayer({
      source: new VectorSource({
        features: [iconFeature],
      }),
    });

    const map = new Map({
      layers: [vectorLayer, markerLayer],
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

    map.on("click", (e) =>
      console.log(
        map.forEachFeatureAtPixel(map.getEventPixel(e.originalEvent), function (feature) {
          return feature;
        })?.ol_uid
      )
    );
    map.on("postrender", function (event) {
      //console.log("hi", event);
      event;
      map.render();
      /* let dynamicStyle = {
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
      });*/
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
