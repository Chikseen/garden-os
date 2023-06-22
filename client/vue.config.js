module.exports = {
	// ...other vue-cli plugin options...
	pwa: {
		name: "GardenOS",
		themeColor: "#f7f7f7",
		background_color: "red",
		appleMobileWebAppCapable: "yes",
		appleMobileWebAppStatusBarStyle: "black",
		icons: [
			{
				src: "img/icons/apple-touch-icon-60x60.png",
				sizes: "60x60",
				type: "image/png",
				purpose: "any",
			},
			{
				src: "img/icons/apple-touch-icon-76x76.png",
				sizes: "76x76",
				type: "image/png",
				purpose: "any",
			},
			{
				src: "img/icons/apple-touch-icon-120x120.png",
				sizes: "120x120",
				type: "image/png",
				purpose: "any",
			},
			{
				src: "img/icons/apple-touch-icon-152x152.png",
				sizes: "152x152",
				type: "image/png",
				purpose: "any",
			},
			{
				src: "img/icons/apple-touch-icon-180x180.png",
				sizes: "180x180",
				type: "image/png",
				purpose: "maskable", // <-- New property value `"maskable"`
			},
		],
	},
};
