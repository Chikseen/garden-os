<template>
  <div id="map"></div>
  <Overlay :data="overlayData" @close="overlayData = null" @fake="fakeRC" />
</template>

<script>
import GeoJSON from "ol/format/GeoJSON.js";
import Map from "ol/Map.js";
import VectorLayer from "ol/layer/Vector.js";
import VectorSource from "ol/source/Vector.js";
import View from "ol/View.js";
import { Fill, Icon, Stroke, Style, Text } from "ol/style.js";
import Feature from "ol/Feature";
import { Point } from "ol/geom.js";

import _marker from "@/assets/map/markers.json";
import { fromLonLat } from "ol/proj";
import Overlay from "@/components/Overlay.vue";

let map;

export default {
  components: { Overlay },
  data() {
    return {
      overlayData: null,
      values: null,
    };
  },
  methods: {
    fakeRC(id, e) {
      // Temp method -> Will be removed later
      this.mapEvent(id, e);
    },
    mapEvent(id, value) {
      console.log(id, value);
      const features = map.getAllLayers()[1].values_.source.getFeatures();
      if (features) {
        const feature = features.find((f) => f.values_.id === id);

        switch (id) {
          case "rain_collector":
            feature.setStyle(this.RainCollector(value).style);
            break;
          case "moisture_I":
            feature.setStyle(this.Moisture_I(value).style);
            break;
          default:
            console.error(`ID: ${id} was not found`);
            break;
        }
      }
    },
    RainCollector(id = 0) {
      var rainCollectorFeature = new Feature({
        type: "Feature",
        geometry: new Point(fromLonLat([11, 47])),
      });

      rainCollectorFeature.setProperties({ id: "rain_collector", name: "Regentonnen", value: id });

      var rainCollectorStyle = new Style({
        image: new Icon({
          anchor: [150, 300],
          anchorXUnits: "pixels",
          anchorYUnits: "pixels",
          opacity: 0.5,
          width: 100,
          height: 100,
          src: "data:image/svg+xml;utf8," + _marker.RainCollector.data[id],
        }),
      });

      rainCollectorFeature.setStyle(rainCollectorStyle);
      return { feature: rainCollectorFeature, style: rainCollectorStyle };
    },
    Moisture_I(id = 0) {
      var moisture_IFeature = new Feature({
        type: "Feature",
        geometry: new Point(fromLonLat([-7,21.5])),
      });

      moisture_IFeature.setProperties({ id: "moisture_I", name: "Bodenfeuchte Sensor am Verande Beet", value: id });

      var moisture_IStyle = new Style({
        image: new Icon({
          anchor: [150, 300],
          anchorXUnits: "pixels",
          anchorYUnits: "pixels",
          opacity: 0.5,
          width: 100,
          height: 100,
          src: "data:image/svg+xml;utf8," + _marker.Moisture_I.data[id],
        }),
      });

      moisture_IFeature.setStyle(moisture_IStyle);
      return { feature: moisture_IFeature, style: moisture_IStyle };
    },
  },
  created() {
    this.emitter.on("Event", (e) => {
      console.log("Incomming Event", e);
      this.mapEvent("rain_collector", e.potiOne);
      this.mapEvent("moisture_I", e.potiTwo);
    });
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
      renderBuffer: 2000, // Render pixels around the view -> how much : YES XD (But forreal check here if there are performance issues)
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
              color: [126, 128, 131],
            }),
            padding: [5, 5, 5, 5],
          })
        );
        return style;
      },
    });

    // Put all marker in one layer
    const markerLayer = new VectorLayer({
      renderBuffer: 2000,
      source: new VectorSource({
        features: [this.RainCollector().feature, this.Moisture_I().feature],
      }),
    });

    const detailedMaplayer = new VectorLayer({
      background: "#1a2b39",
      source: new VectorSource({
        url: "/detailed.json",
        format: new GeoJSON(),
      }),
      renderBuffer: 2000,
      minZoom: 4.5,
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
              color: [126, 128, 131],
            }),
            padding: [5, 5, 5, 5],
          })
        );
        return style;
      },
    });

    // create map with Map and Marker layer
    map = new Map({
      layers: [mainMapLayer, markerLayer, detailedMaplayer],
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
          color: "rgba(126, 128, 131, 0.7)",
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
        this.overlayData = {
          id: feature.values_.id,
          name: feature.values_.name,
          value: feature.values_.value,
        };
        return feature;
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
