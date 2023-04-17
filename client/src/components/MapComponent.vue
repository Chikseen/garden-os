<template>
  <div id="map"></div>
  <Overlay
    :data="overlayData"
    @close="overlayData = null"
    @fake="
      (e) => {
        fake = e;
      }
    "
  />
</template>

<script>
import GeoJSON from "ol/format/GeoJSON.js";
import Map from "ol/Map.js";
import VectorLayer from "ol/layer/Vector.js";
import VectorSource from "ol/source/Vector.js";
import View from "ol/View.js";
import { Fill, Icon, Stroke, Style, Text } from "ol/style.js";
import Feature from "ol/Feature";
import { Geometry, Point } from "ol/geom.js";
import Layer from "ol/layer/Layer.js";
import { composeCssTransform } from "ol/transform";

import _marker from "@/assets/map/markers.json";
import { fromLonLat } from "ol/proj";
import Overlay from "@/components/Overlay.vue";

export default {
  components: { Overlay },
  data() {
    return {
      overlayData: null,
      fake: 0,
    };
  },
  methods: {
    RainCollectorMarker() {
      var rainCollector = _marker.RainCollector.init;

      var rainCollectorFeature = new Feature({
        type: "Feature",
        geometry: new Point(fromLonLat([11, 48])),
      });

      rainCollectorFeature.setProperties({ id: "rain_collector", name: "Regentonnen" });

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
      style: (feature) => {
        let color = feature.get("color") || "#eeeeee";
        let name = feature.get("name") || "";

        if (feature.get("id") === "main") name = "";

        style.getFill().setColor(color);
        style.setText(
          new Text({
            font: "16px sans-serif",
            textAlign: "center",
            justify: "left",
            text: `${name}`,
            fill: new Fill({
              color: [255, 255, 255, 1],
            }),
            /*backgroundFill: new Fill({
              color: [168, 50, 153, 0.6],
            }),*/
            padding: [5, 5, 5, 5],
          })
        );
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

      if (feature?.get("id") === "rain_collector") {
        console.log(this.fake);
        var rainCollector = _marker.RainCollector.data[this.fake];

        feature.setStyle(
          new Style({
            image: new Icon({
              anchor: [100, 100],
              anchorXUnits: "pixels",
              anchorYUnits: "pixels",
              opacity: 0.5,
              width: 100,
              height: 100,
              src: "data:image/svg+xml;utf8," + rainCollector,
            }),
          })
        );
        /*
                  console.log("hi", feature.getStyle().getImage());

                  feature.getStyle().getImage().setAnchor([25, -150]);
                  feature.getStyle().getImage().setHeight(50);
                  feature.getStyle().getImage().setWidth(75);*/
      }
    });

    // Add map Event listner
    map.on("click", (e) => {
      this.overlayData = null;
      map.forEachFeatureAtPixel(map.getEventPixel(e.originalEvent), (feature) => {
        console.log(feature);
        this.overlayData = {
          id: feature.values_.id,
          name: feature.values_.name,
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

svg {
  width: 25%;
}
circle {
  fill: green;
  stroke: red;
}
#outer {
  fill: blue;
}
.inner {
  fill: red;
}
.hidden {
  display: none;
}

.icon--blue circle {
  fill: #0000ff !important;
}
.icon--blue .cat {
  display: none;
}
</style>
