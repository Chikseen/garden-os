<template>
  <div class="fuelMeter_wrapper">
    <span class="fuelMeter_wrapper_background" :style="`bottom: ${(value * 100) / 255}%`"></span>
    <p class="fuelMeter_text">{{ value }}</p>
  </div>
</template>

<script>
export default {
  props: {
    value: { type: Number, default: 2.5 },
  },
};
</script>

<style lang="scss">
.fuelMeter {
  &_wrapper {
    position: relative;
    width: 300px;
    padding-bottom: 125%;
    background-color: rgba(0, 255, 255);
    border-radius: 15px;
    transform: perspective(500px) rotateX(-45deg) translateZ(-500px) translateY(225px);
    overflow: hidden;

    &_background {
      position: absolute;
      left: 0;
      height: 200%;
      width: 150%;
      background: rgb(23, 173, 81);
      z-index: -10;
      transition: all 0.5s;
    }
  }

  &_text {
    margin: 0;
    height: 100%;
    font-size: 4rem;
    vertical-align: middle;
    text-align: center;
    z-index: 10;
  }
}
</style>
