<template>
  <v-card
    class="content-bg card mx-auto"
    :max-width="card.maxWidth"
    flat
    tile
    router
    :to="`watch?v=` + video.id"
  >
    <v-img :src="thumbnail" height="200px"></v-img>
    <v-row no-gutters>
      <v-col v-if="card.type != 'noAvatar'" cols="2">
        <v-list-item class="pl-0 pt-3" router :to="channel.id">
          <v-list-item-avatar color="grey darken-3">
            <v-img class="elevation-6" :src="channelAvatar"></v-img>
          </v-list-item-avatar>
        </v-list-item>
      </v-col>
      <v-col>
        <v-card-title class="pl-2 pt-3 subtitle-1 font-weight-bold">
          {{ video.title }}
        </v-card-title>

        <v-card-subtitle class="pl-2 pb-0">
          {{ channel.name }}
        </v-card-subtitle>

        <v-card-subtitle class="pl-2 pt-0">
          {{ video.views }} views<v-icon>mdi-circle-small</v-icon>{{ timeAgo }}
        </v-card-subtitle>
      </v-col>
    </v-row>
  </v-card>
</template>

<script>
import * as timeago from "timeago.js";

export default {
  props: {
    card: {
      type: Object,
      required: true,
    },
    channel: {
      type: Object,
      required: true,
    },
    video: {
      type: Object,
      required: true,
    },
  },

  computed: {
    thumbnail() {
      return `${process.env.BASE_URL}data/thumbnail/${this.video.thumbnail}`;
    },
    channelAvatar() {
      return `${process.env.BASE_URL}data/avatar/${this.channel.id}.jpg`;
    },
    timeAgo() {
      return timeago.format(this.video.uploadDate);
    },
  },
};
</script>

<style></style>
