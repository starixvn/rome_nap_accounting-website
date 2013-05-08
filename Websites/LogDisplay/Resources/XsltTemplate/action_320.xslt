<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="lrf_customerupdmember_minmaxbet">
    <table class="tblDesc">
      <tr>
        <td class="colW1">{lbl:Custid}</td>
        <td class="colW2">
          <xsl:value-of select="@custid" />
        </td>
        <td class="colW1">{lbl:MinBet}</td>
        <td class="colW2">
          <xsl:value-of select="@minb" />
        </td>
        <td class="colW1">{lbl:MaxBet}</td>
        <td class="colW2">
          <xsl:value-of select="@maxb" />
        </td>
        
      </tr>
      <tr>
        <td class="colW1">{lbl:MaxPerMatch}</td>
        <td class="colW2">
          <xsl:value-of select="@mpm" />
        </td>
        <td class="colW1">{lbl:minbv2}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv2" />
        </td>
        <td class="colW1">{lbl:maxbv2}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv2" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv2}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv2" />
        </td>
        <td class="colW1">{lbl:minbv3}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv3" />
        </td>
        <td class="colW1">{lbl:maxbv3}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv3" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv3}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv3" />
        </td>
        <td class="colW1">{lbl:minbv5}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv5" />
        </td>
        <td class="colW1">{lbl:maxbv5}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv5" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv5}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv5" />
        </td>
        <td class="colW1">{lbl:minbv8}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv8" />
        </td>
        <td class="colW1">{lbl:maxbv8}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv8" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv8}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv8" />
        </td>
        <td class="colW1">{lbl:minbv10}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv10" />
        </td>
        <td class="colW1">{lbl:maxbv10}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv10" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv10}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv10" />
        </td>
        <td class="colW1">{lbl:minbv11}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv11" />
        </td>
        <td class="colW1">{lbl:maxbv11}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv11" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv11}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv11" />
        </td>
        <td class="colW1">{lbl:minbv99}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv99" />
        </td>
        <td class="colW1">{lbl:maxbv99}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv99" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mpmv99}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv99" />
        </td>
        <td class="colW1">{lbl:MinBv99MP}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv99mp" />
        </td>
        <td class="colW1">{lbl:MaxBv99MP}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv99mp" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:MPMv99MP}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv99mp" />
        </td>
        <td class="colW1">{lbl:MinBv161}</td>
        <td class="colW2">
          <xsl:value-of select="@minbv161" />
        </td>
        <td class="colW1">{lbl:MaxBv161}</td>
        <td class="colW2">
          <xsl:value-of select="@maxbv161" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:MPMv161}</td>
        <td class="colW2">
          <xsl:value-of select="@mpmv161" />
        </td>
        <td class="colW1">{lbl:MPBv161}</td>
        <td class="colW2">
          <xsl:value-of select="@mpbv161" />
        </td>
        <td class="colW1">{lbl:updmultiple}</td>
        <td class="colW2">
          <xsl:value-of select="@updmultiple" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:ummb}</td>
        <td class="colW2">
          <xsl:value-of select="@ummb" />
        </td>
        <td class="colW1">{lbl:ummbv2}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv2" />
        </td>
        <td class="colW1">{lbl:ummbv3}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv3" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:ummbv5}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv5" />
        </td>
        <td class="colW1">{lbl:ummbv8}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv8" />
        </td>
        <td class="colW1">{lbl:ummbv10}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv10" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:ummbv11}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv11" />
        </td>
        <td class="colW1">{lbl:ummbv99}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv99" />
        </td>
        <td class="colW1">{lbl:UMMBv99MP}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv99mp" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:UMMBv161}</td>
        <td class="colW2">
          <xsl:value-of select="@ummbv161" />
        </td>
        <td class="colW1">{lbl:subaccname}</td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">{lbl:userID}</td>
        <td class="colW2">
          <xsl:value-of select="@userid" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:language}</td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">{lbl:IP}</td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        <td class="colW1">{lbl:ErrCode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:ErrMsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1"></td>
        <td class="colW2">
        </td>
        <td class="colW1"></td>
        <td class="colW2">
        </td>
      </tr>
    </table>
  </xsl:template>



</xsl:stylesheet>
