<template>
  <div id="map"></div>
  <Overlay :data="overlayData" @close="overlayData = null" />
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
import { fromLonLat } from "ol/proj";
import Overlay from "@/components/Overlay.vue";

export default {
  components: { Overlay },
  data() {
    return {
      overlayData: null,
    };
  },
  methods: {
    RainCollectorMarker() {
      var rainCollector = _marker.RainCollector.svg;

      var rainCollectorFeature = new Feature({
        type: "Feature",
        geometry: new Point(fromLonLat([11, 48])),
      });

      rainCollectorFeature.setProperties({ id: "rain_collector" });

      var rainCollectorStyle = new Style({
        image: new Icon({
          anchor: [100, 100],
          anchorXUnits: "pixels",
          anchorYUnits: "pixels",
          opacity: 0.5,
          width: 100,
          height: 100,
          src: "data:image/svg+xml;utf8," + rainCollector,
        }),
      });

      rainCollectorFeature.setStyle(rainCollectorStyle);
      return rainCollectorFeature;
    },
  },
  mounted() {
    const style = new Style({
      fill: new Fill({
        color: "#eeeeee",
      }),
    });

    // Set static main map
    const mainMapLayer = new VectorLayer({
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

    // Put all marker in one layer
    const markerLayer = new VectorLayer({
      source: new VectorSource({
        features: [this.RainCollectorMarker()],
      }),
    });

    // create map with Map and Marker layer
    const map = new Map({
      layers: [mainMapLayer, markerLayer],
      target: "map",
      view: new View({
        center: [0, 0],
        zoom: 3.2,
        minZoom: 3.1,
      }),
    });

    // Add userInteraction
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
    map.on("pointermove", (evt) => {
      if (evt.dragging) {
        return;
      }
      const pixel = map.getEventPixel(evt.originalEvent);

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
    });

    // Add map Event listner
    map.on("click", (e) => {
      this.overlayData = null;
      map.forEachFeatureAtPixel(map.getEventPixel(e.originalEvent), (feature) => {
        console.log(feature);
        this.overlayData = {
          id: feature.values_.id,
        };
        return feature;
      });
    });
    map.on("postrender", (e) => {
      /*
      e;
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

      mainMapLayer.setStyle((e) => {
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
