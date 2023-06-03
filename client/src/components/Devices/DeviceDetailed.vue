<template>
  <div class="device_wrapper">
    <p> {{ device.name }} </p>
    <p> {{ device.value }} </p>
    <p> {{ device.date }} </p>
  </div>
</template>

<script>

export default {
  props: {
    device: {
      type: Object,
      default: () => { }
    }
  }
}
</script>

<style lang="scss">
.device {
  &_wrapper {
    display: flex;
    flex-direction: column;
    background-color: whitesmoke;
    margin: 10px;
    max-width: 250px;
    border-radius: 10px;
    box-shadow: 0 0 5px 5px #5f5e5e21;
    padding: 5px;

    p {
      text-align: center;
    }
  }
}
</style>