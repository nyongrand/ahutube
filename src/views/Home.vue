<template>
  <div id="home" class="pa-4">
    <v-container fluid>
      <h3 class="headline font-weight-medium">Recommended</h3>
      <v-row>
        <v-col
          v-for="i in recomendations.length"
          :key="i"
          cols="12"
          sm="6"
          md="4"
          lg="3"
          class="mx-xs-auto"
        >
          <v-skeleton-loader type="card-avatar" :loading="loading">
            <video-card
              :card="{ maxWidth: 350 }"
              :channel="getChannel(recomendations[i - 1].uploaderId)"
              :video="recomendations[i - 1]"
            ></video-card>
          </v-skeleton-loader>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
import axios from "axios";
import videoCard from "@/components/VideoCard";

export default {
  name: "Home",
  components: {
    videoCard,
  },
  data: () => ({
    loading: true,
    channels: [],
    recomendations: [],
  }),

  async mounted() {
    // load channels list
    const res1 = await axios.get(`/data/channels.json`);
    this.channels = res1.data;

    // get all video list
    const res2 = await Promise.all(
      this.channels.map((x) => axios.get(`/data/channels/${x.id}/videos.json`))
    );

    // get random video recomendations
    for (let i = 0; i <= 12; i++) {
      const c = this.getRandomInt(this.channels.length);
      const channelVideos = res2[c].data;

      const v = this.getRandomInt(channelVideos.length);
      this.recomendations.push(channelVideos[v]);
    }

    this.loading = false;
  },

  methods: {
    getChannel(id) {
      return this.channels.find((x) => x.id === id);
    },

    getRandomInt(max) {
      max = Math.floor(max);
      return Math.floor(Math.random() * max);
    },
  },
};
</script>

<style lang="scss">
.card {
  background: #f9f9f9 !important;
}
</style>
