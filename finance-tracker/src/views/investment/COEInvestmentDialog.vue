<template>
  <el-dialog
    :model-value="isOpen"
    :title="editInvestmentId ? 'Cập nhật mã cổ phiếu' : 'Tạo mới mã cổ phiếu'"
    width="30%"
    @close="closeModel(false)"
  >
    <div class="space-y-3">
      <div class="flex flex-col">
        <label>Kênh đầu tư</label>
        <el-select
          v-model="investmentData.channelId"
          placeholder="Choose channel"
        >
          <el-option
            v-for="channel in listChannel"
            :key="channel.id"
            :label="channel.code + '-' + channel.name"
            :value="channel.id"
          ></el-option>
        </el-select>
      </div>
      <div>
        <label>Mã cổ phiếu</label>
        <el-input
          v-model="investmentData.stockCode"
          placeholder="Nhập mã cổ phiếu"
        ></el-input>
      </div>
      <div>
        <label>Tên công ty</label>
        <el-input
          v-model="investmentData.companyName"
          placeholder="Tên công ty"
        ></el-input>
      </div>
      <div>
        <label>Giá hiện thời</label>
        <el-input v-model="investmentData.marketPrice" type="number"></el-input>
      </div>
      <div>
        <label>Mô tả</label>
        <el-input
          v-model="investmentData.description"
          type="textarea"
        ></el-input>
      </div>
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="closeModel(false)">Cancel</el-button>
        <el-button type="primary" @click="save">Confirm</el-button>
      </span>
    </template>
  </el-dialog>
</template>
<script lang="ts" setup>
import { CreateOrUpdateInvestmentDto } from "@/models/investment/InvestmentDtos";
import { ChannelSellectDto } from "@/models/investment-channel/InvestmentChannelModels";
import financeService from "@/services/investment.service";
import investmentChannelService from "@/services/investment-channel.service";
import { appStore } from "@/store/appStore";
import { ElNotification } from "element-plus";
import {
  ref,
  defineProps,
  defineEmits,
  withDefaults,
  onUpdated,
  onBeforeMount,
} from "vue";
import { storeToRefs } from "pinia";
// store
const store = appStore();
const { channel } = storeToRefs(store);
//data
let investmentData = ref<CreateOrUpdateInvestmentDto>({});
let listChannel = ref<ChannelSellectDto[]>();
// Prop
const props = withDefaults(
  defineProps<{ isOpen: boolean; editInvestmentId: number | null }>(),
  {
    isOpen: false,
    editInvestmentId: null,
  },
);

// event
onBeforeMount(() => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
    if (listChannel.value.length > 0) {
      investmentData.value.channelId = listChannel.value[0].id;
    }
  });
});

// watcher
// watch(
//   () => props.editInvestmentId,
//   (newValue, oldValue) => {
//     if (newValue) {
//       getInvestmentById(newValue);
//     } else {
//       investmentData.value = {};
//     }
//   },
// );

// Event
const emits = defineEmits(["close"]);

// Hook
onUpdated(() => {
  if (props.editInvestmentId && props.isOpen) {
    getInvestmentById(props.editInvestmentId);
  } else if (!props.editInvestmentId && props.isOpen) {
    investmentData.value = {};
    investmentData.value.channelId = channel.value.id;
  }
});

// Method
function getInvestmentById(id: number) {
  financeService.getInvestemntById(id).then((res) => {
    investmentData.value = res.result;
  });
}
async function save() {
  if (props.editInvestmentId) {
    financeService.editInvestment(investmentData.value).then((res) => {
      if (res.success) {
        ElNotification({
          title: "Success",
          message: "Cập nhật thành công",
          type: "success",
        });
        closeModel(true);
      } else {
        ElNotification({
          title: "Error",
          message: result.error.message,
          type: "error",
        });
        closeModel(false);
      }
    });
  } else {
    var result = await financeService.createInvestment(investmentData.value);
    if (!result.success) {
      ElNotification({
        title: "Error",
        message: result.error.message,
        type: "error",
      });

      closeModel(false);
    } else {
      ElNotification({
        title: "Success",
        message: "Thêm mới thành công",
        type: "success",
      });
      closeModel(true);
    }
  }
}

// close model
function closeModel(isSuccess: boolean) {
  emits("close", isSuccess);
}
</script>
