<template>
      <TheFooter>
        <div>
          <!--footer-->
          <a href="https://sevensins.com">SevenSins</a>
          <span class="ml-1">&copy; 2020 creativeWebsHQ.</span>
        </div>

      </TheFooter>
</template>
<script>
import { Footer as TheFooter } from '@coreui/vue'
export default {
  name: 'DefaultFooter',
  components: {
    TheFooter
  },
}
</script>
